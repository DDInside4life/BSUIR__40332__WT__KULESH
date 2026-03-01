using System;
using System.Collections.Generic;
using System.Text;

namespace KULESH.Domain.Models
{
    public class ResponseData<T>
    {
        public ResponseData()
        { }
        // запрашиваемые данные 
        public T? Data { get; set; }
        // признак успешного завершения запроса 
        public bool Success { get; set; } = true;
        // сообщение в случае неуспешного завершения 
        public string? ErrorMessage { get; set; } = string.Empty;
        /// <summary> 
        /// Успешный ответ 
        /// </summary> 
        /// <param name="data">передаваемые данные</param> 
        /// <returns></returns> 
        public static ResponseData<T> OK(T? data)
        {
            return new ResponseData<T> { Data = data };
        }
        /// <summary> 
        /// Ошибка 
        /// </summary> 
        /// <param name="message">Текст сообщения об ошибке</param> 
        /// <returns></returns> 
        public static ResponseData<T> Error(string message)
        {
            return new ResponseData<T>
            {
                ErrorMessage = message,
                Success = false,
                Data = (T)Activator.CreateInstance(typeof(T))
            };
        }
    }

}
