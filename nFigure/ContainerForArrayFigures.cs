namespace nFigure
{
    /// <summary>
    /// Класс, на основе полей которого создаётся таблица в БД.
    /// </summary>
    public class ContainerForArrayFigures
    {
        public int Id { get; set; }

        public byte[] BuffContainer { get; set; }
    }
}
