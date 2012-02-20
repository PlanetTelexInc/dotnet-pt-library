﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace PlanetTelex.Serialization
{
    /// <summary>
    /// An XML serializer for data contracts.
    /// </summary>
    public class DataContractXmlSerializer : ISerializer
    {
        #region Implementation of ISerializer

        /// <summary>
        /// Serializes an object to a string.
        /// </summary>
        /// <typeparam name="T">The type of object being serialized.</typeparam>
        /// <param name="instance">The object to serialize.</param>
        /// <returns>A string.</returns>
        public string Serialize<T>(T instance)
        {
            StringBuilder builder = new StringBuilder();

            using (XmlWriter xmlWriter = XmlWriter.Create(builder))
            using (XmlDictionaryWriter writer = XmlDictionaryWriter.CreateDictionaryWriter(xmlWriter))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(writer, instance);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Deserializes a string into an object.
        /// </summary>
        /// <typeparam name="T">The type of object being deserialized.</typeparam>
        /// <param name="serializedInstance">A string representation of the object type specified.</param>
        /// <returns>A new object of the type specified.</returns>
        public T Deserialize<T>(string serializedInstance)
        {
            T instance;

            using (XmlReader xmlReader = XmlReader.Create(new StringReader(serializedInstance)))
            using (XmlDictionaryReader reader = XmlDictionaryReader.CreateDictionaryReader(xmlReader))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                instance = (T)serializer.ReadObject(reader);
            }

            return instance;
        }

        /// <summary>
        /// Determines if the instance is serializable.
        /// </summary>
        /// <typeparam name="T">The type of object being checked.</typeparam>
        /// <param name="instance">Object instance to be checked.</param>
        /// <returns><c>true</c> if this instance is serializable; otherwise, <c>false</c>.</returns>
        public bool IsSerializable<T>(T instance) where T : class, new()
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            Attribute[] attributes = Attribute.GetCustomAttributes(typeof(T));
            return attributes.OfType<DataContractAttribute>().Any();
        }

        /// <summary>
        /// Serializes and deserializes an object instance to test that the class is correctly configured for serialization.
        /// </summary>
        /// <typeparam name="T">The type of object being serialized.</typeparam>
        /// <param name="instance">Object instance to be checked.</param>
        /// <returns>A new object instance.</returns>
        public T RoundtripSerialize<T>(T instance) where T : class, new()
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            return Deserialize<T>(Serialize(instance));
        }

        #endregion
    }
}
