using UnityEngine;

public class PlayerHandsSwayScript : MonoBehaviour
{
    [Header("Sway settings")]
    [SerializeField] protected float swayClamp;
    [SerializeField] protected float smoothing;
    protected Vector3 origin;

    private void Start()
    {
        origin = transform.localPosition;
    }

    void Update()
    {
        Sway();
    }

    protected virtual void Sway()
    {
        // Получаем вектор движения мыши по горизонтали и вертикали
        Vector2 input = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // Ограничиваем движение мыши
        input.x = Mathf.Clamp(input.x, -swayClamp, swayClamp);
        input.y = Mathf.Clamp(input.y, -swayClamp, swayClamp);

        //целевой вектор, который будет определять, как объект будет смещаться
        Vector3 target = new Vector3(-input.x, -input.y, 0);

        // Плавно изменяем локальную позицию объекта, чтобы он реагировал на движение мыши
        transform.localPosition = Vector3.Lerp(transform.localPosition, target + origin, Time.deltaTime * smoothing);
    }
}
