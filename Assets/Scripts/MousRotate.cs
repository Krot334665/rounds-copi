using UnityEngine;

public class MousRotate : MonoBehaviour
{
    // Установите желаемую высоту объекта относительно экрана
    public float height = 1.0f;
    void Update()
    {
        // Получаем позицию мыши в экранных координатах
        Vector3 mousePosition = Input.mousePosition;

        // Добавляем высоту относительно экрана
        mousePosition.z = height;

        // Преобразуем позицию мыши из экранных в мировые координаты
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Находим направление курсора от объекта
        Vector3 direction = targetPosition - transform.position;

        // Находим угол поворота в радианах
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Если угол поворота больше 180 градусов, отзеркаливаем объект
        if (Mathf.Abs(angle) > 90)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            angle -= 180; // Корректируем угол после отзеркаливания
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Создаем кватернион поворота по оси Z
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Применяем поворот к объекту
        transform.rotation = rotation;

    }
}