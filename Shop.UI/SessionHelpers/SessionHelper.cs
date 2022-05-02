using Newtonsoft.Json;

namespace Shop.UI.SessionHelpers
{
    public static class SessionHelper
    {

        /// <summary>
        /// ISession.SetString() method extension to serialize using JSON the complex object passed as the value parameter and store it in the session
        /// </summary>
        /// <typeparam name="T">the complex object type passed to be stored</typeparam>
        /// <param name="session">this ISession: HttpContext.Session</param>
        /// <param name="key">the string key to identify the key/value pair containing the complex object to add</param>
        /// <param name="value"> the T type object to be stored in the session cache</param>
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }


        /// <summary>
        /// ISession.GetString() method extension to read the JSON string value for the key passed as the key parameter, deserializing it using JSON, 
        /// to returning the value associated with the key parameter
        /// </summary>
        /// <typeparam name="T">the complex object type to be read</typeparam>
        /// <param name="session">this ISession: HttpContext.Session</param>
        /// <param name="key">the string key to identify the key/value pair containing the complex object to retrieve</param>
        /// <returns>
        /// 1. if there is no key/value pair in the Session object for the provided string key: it returns the default value of the complex object: null.
        /// 2. If there is a key/value pair in the Session object for the provided string key: it returns the complex object deserialized using JSON.
        /// </returns>
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

    }

}  