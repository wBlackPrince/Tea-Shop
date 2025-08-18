namespace Tea_Shop.Shared;

public enum ErrorType
{
    /// <summary>
    /// неизвестная ошибка
    /// </summary>
    None,

    /// <summary>
    /// ошибка валидации
    /// </summary>
    VALIDATION,

    /// <summary>
    /// ошибка ничего не найдено
    /// </summary>
    NOT_FOUND,

    /// <summary>
    /// ошибка сервера
    /// </summary>
    FAILURE,

    /// <summary>
    /// ошибка-конфликт
    /// </summary>
    CONFLICT
}