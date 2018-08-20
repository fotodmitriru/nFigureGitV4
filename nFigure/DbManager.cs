using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.EntityFrameworkCore;

namespace nFigure
{
    /// <summary>
    /// Перечисление, которое содержит возможные состояния при работе с БД.
    /// </summary>
    public enum StatusDb
    {
        Failed,
        Ok,
        ErrorId,
        ErrorConnected,
        ErrorIO
    }
    /// <summary>
    /// Структура, содержащая список возможных сообщений при работе с БД.
    /// </summary>
    public struct StatusDbMsg
    {
        /// <summary>
        /// Массив сообщений.
        /// </summary>
        public static string[] Msg { get; } =
        {
            "Не удалось прочитать данные из БД!", "OK", "Указанный ID не существует!",
            $"Не удалось установить соединеие с БД!", "Ошибка входных данных!"
        };
    }

    /// <inheritdoc />
    /// <summary>
    /// Класс для работы с БД. Позволяет сохранять и загружать данные из БД.
    /// </summary>
    internal class DbManager : IDisposable
    {
        /// <summary>
        /// Метод сохраняет массив объектов в БД. Возвращает StatusDb.
        /// </summary>
        /// <param name="arrayOfObjects">массив объектов, которые будут сохранены в БД</param>
        /// <returns>Возвращает StatusDb</returns>
        public StatusDb SaveToDb(Object[] arrayOfObjects)
        {
            if (arrayOfObjects == null)
                return StatusDb.ErrorIO;
            BinaryFormatter formatter = new BinaryFormatter();
            ContainerForArrayFigures figCont = new ContainerForArrayFigures();
            MemoryStream mStream = new MemoryStream();
            formatter.Serialize(mStream, arrayOfObjects);

            using (ConfigureDb db = new ConfigureDb())
            {
                try
                {
                    db.Database.Migrate();
                    figCont.BuffContainer = mStream.ToArray();
                    db.FiguresContainer.Add(figCont);
                    return (StatusDb) db.SaveChanges();
                }
                catch (Npgsql.PostgresException)
                {
                    return StatusDb.Failed;
                }
            }
        }

        /// <summary>
        /// Метод принимает массив типа int[] и записывает в него Id всех записей из таблицы БД. 
        /// </summary>
        /// <returns>Возвращает StatusDb</returns>
        public StatusDb ViewDb(ref int[] entryArrayId)
        {
            using (ConfigureDb db = new ConfigureDb())
            {
                try
                {
                    List<ContainerForArrayFigures> entryArrayFigureses = db.FiguresContainer.ToList();
                    entryArrayId = new int[entryArrayFigureses.Count];
                    for (int i = 0; i < entryArrayFigureses.Count; i++)
                    {
                        entryArrayId[i] = entryArrayFigureses[i].Id;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is Npgsql.PostgresException)
                        return StatusDb.Failed;
                    if (ex is System.Net.Sockets.SocketException)
                        return StatusDb.ErrorConnected;
                }
            }

            return StatusDb.Ok;
        }

        /// <summary>
        /// Метод загружает данные из таблицы БД.
        /// </summary>
        /// <param name="id">запись в таблице, которую нужно загрузить</param>
        /// <param name="arrayOfObjects">массив типа Object[], в который будут записаны объекты, загруженные из таблицы БД</param>
        /// <returns>Возвращает StatusDb</returns>
        public StatusDb LoadFromDb(int id, out Object[] arrayOfObjects)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (ConfigureDb db = new ConfigureDb())
            {
                arrayOfObjects = null;
                try
                {
                    if (!db.FiguresContainer.Any())
                        return StatusDb.Failed;

                    ContainerForArrayFigures entryArrayFigureses = db.FiguresContainer.Find(id);
                    if (entryArrayFigureses == null)
                        return StatusDb.ErrorId;

                    MemoryStream mStream = new MemoryStream(entryArrayFigureses.BuffContainer);
                    arrayOfObjects = (Object[]) formatter.Deserialize(mStream);
                }
                catch (Exception ex)
                {
                    if (ex is Npgsql.PostgresException)
                        return StatusDb.Failed;
                    if (ex is System.Net.Sockets.SocketException)
                        return StatusDb.ErrorConnected;
                }
            }

            if (arrayOfObjects == null)
                return StatusDb.Failed;

            return StatusDb.Ok;
        }

        public void Dispose()
        {
            //
        }
    }
}
