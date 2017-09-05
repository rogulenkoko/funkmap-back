


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Tools
{
    public static class EntityUpdateExtension
    {
        public static T FillEntity<T>(this T entity, T newEntity)
        {
            var emptyInstance = Activator.CreateInstance(typeof(T));

            foreach (var propertyInfo in entity.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(entity)?.GetType() == typeof(GeoJsonPoint<GeoJson2DGeographicCoordinates>))
                {
                    var value = propertyInfo.GetValue(entity) as GeoJsonPoint<GeoJson2DGeographicCoordinates>;
                    var newValue = propertyInfo.GetValue(newEntity) as GeoJsonPoint<GeoJson2DGeographicCoordinates>;
                    if (newValue == null)
                    {
                        continue;
                    }
                    else
                    {
                        if (value.Coordinates.Longitude != newValue.Coordinates.Longitude &&
                            value.Coordinates.Latitude != newValue.Coordinates.Latitude)
                        {
                            propertyInfo.SetValue(entity, propertyInfo.GetValue(newEntity));
                        }
                        continue;
                    }

                }

                if (propertyInfo.GetValue(entity)?.GetType() == typeof(BsonBinaryData))
                {
                    var value = propertyInfo.GetValue(entity) as BsonBinaryData;
                    var newValue = propertyInfo.GetValue(newEntity) as BsonBinaryData;

                    if(newValue == null) continue;
                    else
                    {
                        if (!CompareObjects(value?.AsByteArray, newValue?.AsByteArray))
                        {
                            propertyInfo.SetValue(entity, propertyInfo.GetValue(newEntity));
                        }
                    }
                }


                if (!CompareObjects(propertyInfo.GetValue(newEntity), propertyInfo.GetValue(emptyInstance)))
                {
                    propertyInfo.SetValue(entity, propertyInfo.GetValue(newEntity));
                }
            }

            return entity;
        }


        public static bool CompareObjects<T>(T expectInput, T actualInput)
        {
            if (typeof(T).IsPrimitive)
            {
                if (expectInput.Equals(actualInput))
                {
                    return true;
                }

                return false;
            }

            if (expectInput is IEquatable<T>)
            {
                if (expectInput.Equals(actualInput))
                {
                    return true;
                }

                return false;
            }

            if (expectInput is IComparable)
            {
                if (((IComparable)expectInput).CompareTo(actualInput) == 0)
                {
                    return true;
                }

                return false;
            }
            
            if (expectInput is IEnumerable)
            {
                if (actualInput == null) return false;
                var expectEnumerator = ((IEnumerable)expectInput).GetEnumerator();
                var actualEnumerator = ((IEnumerable)actualInput).GetEnumerator();

                var canGetExpectMember = expectEnumerator.MoveNext();
                var canGetActualMember = actualEnumerator.MoveNext();

                while (canGetExpectMember && canGetActualMember && true)
                {
                    var currentType = expectEnumerator.Current.GetType();
                    object isEqual = typeof(Utils).GetMethod("CompareObjects").MakeGenericMethod(currentType).Invoke(null, new object[] { expectEnumerator.Current, actualEnumerator.Current });

                    if ((bool)isEqual == false)
                    {
                        return false;
                    }

                    canGetExpectMember = expectEnumerator.MoveNext();
                    canGetActualMember = actualEnumerator.MoveNext();
                }

                if (canGetExpectMember != canGetActualMember)
                {
                    return false;
                }

                return true;
            }
            
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var expectValue = typeof(T).GetProperty(property.Name).GetValue(expectInput);
                var actualValue = typeof(T).GetProperty(property.Name).GetValue(actualInput);

                if (expectValue == null || actualValue == null)
                {
                    if (expectValue == null && actualValue == null)
                    {
                        continue;
                    }

                    return false;
                }

                object isEqual = typeof(Utils).GetMethod("CompareObjects").MakeGenericMethod(property.PropertyType).Invoke(null, new object[] { expectValue, actualValue });

                if ((bool)isEqual == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
