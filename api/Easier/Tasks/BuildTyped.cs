﻿using System;
using System.Data;
using Simpler;

namespace Easier.Tasks
{
    public class BuildTyped<T> : InOutTask<BuildTyped<T>.Input, BuildTyped<T>.Output> 
    {
        public class Input
        {
            public IDataRecord DataRecord { get; set; }
        }

        public class Output
        {
            public T Object { get; set; }
        }

        public override void Execute()
        {
            Out.Object = (T)Activator.CreateInstance(typeof(T));
            var objectType = typeof(T);

            for (var i = 0; i < In.DataRecord.FieldCount; i++)
            {
                var columnName = In.DataRecord.GetName(i);
                var propertyInfo = objectType.GetProperty(columnName);

                if (propertyInfo == null) throw new BuildTypedException(String.Format(
                    "The DataRecord contains column '{0}' that is not a property of the '{1}' class.",
                    columnName,
                    objectType.FullName
                ));

                var columnValue = In.DataRecord[columnName];
                if (columnValue.GetType() != typeof(DBNull))
                {
                    var propertyType = propertyInfo.PropertyType;

                    if (propertyType.IsEnum)
                    {
                        propertyInfo.SetValue(Out.Object,Enum.Parse(propertyType,columnValue.ToString()),null);
                        continue;
                    }

                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                    }

                    columnValue = Convert.ChangeType(columnValue, propertyType);
                    propertyInfo.SetValue(Out.Object, columnValue, null);
                }
            }
        }
    }
}
