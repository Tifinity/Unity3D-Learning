using UnityEngine;

public class Halo : MonoBehaviour {
    private ParticleSystem particleSys;                             // 粒子系统
    private ParticleSystem.Particle[] particleArr;                  // 粒子数组
    private CirclePosition[] circle;                                // 极坐标数组
    public int pCount = 5000;                                       // 粒子数量
    public float pSize = 0.15f;                                     // 粒子大小
    public float minRadius = 4.0f;                                  // 最小半径
    public float maxRadius = 11.0f;                                 // 最大半径
    public bool clockwise = true;                                   // 旋转方向
    public float speed = 1f;                                        // 速度
    public float pingPong = 0.01f;                                  // 游离范围
    public int tier = 10;                                           // 速度差分层数

    public Camera camera;                                           // 主摄像机
    private Ray ray;                                                // 射线
    private RaycastHit hit;
   
    private float[] before;                                         // 收缩前粒子位置
    private float[] after;                                          // 收缩后粒子位置
    public float shrinkSpeed = 5f;                                  // 粒子缩放的速度
    private bool ischange = false;                                  // 是否收缩

    public Gradient colorGradient;                                  // 颜色渐变
    private GradientAlphaKey[] alphaKeys;                           // 透明度
    private GradientColorKey[] colorKeys;                           // 颜色

    void Start() {   
        particleArr = new ParticleSystem.Particle[pCount];
        circle = new CirclePosition[pCount];
        before = new float[pCount];
        after = new float[pCount];

        // 初始化粒子系统
        particleSys = this.GetComponent<ParticleSystem>();
        var main = particleSys.main;
        main.startSpeed = 0;
        main.startSize = pSize;
        main.loop = false;
        main.maxParticles = pCount;                                  // 粒子数量               
        particleSys.Emit(pCount);                                    // 发射粒子
        particleSys.GetParticles(particleArr);

        alphaKeys = new GradientAlphaKey[5];
        colorKeys = new GradientColorKey[2];
        
        // 初始化梯度颜色控制器
        alphaKeys[0].time = 0.0f; alphaKeys[0].alpha = 0.2f;
        alphaKeys[1].time = 0.25f; alphaKeys[1].alpha = 1f;
        alphaKeys[2].time = 0.5f; alphaKeys[2].alpha = 0.2f;
        alphaKeys[3].time = 0.75f; alphaKeys[3].alpha = 1f;
        alphaKeys[4].time = 1.0f; alphaKeys[4].alpha = 0.2f;
        
        if(clockwise) {
            colorKeys[0].time = 0.25f; colorKeys[0].color = new Color(221f / 255, 49f / 255, 221f / 255);
            colorKeys[1].time = 0.75f; colorKeys[1].color = new Color(24f / 255, 177f / 255, 224f / 255);
        }
        else {
            colorKeys[0].time = 0.25f; colorKeys[0].color = new Color(24f / 255, 177f / 255, 224f / 255);
            colorKeys[1].time = 0.75f; colorKeys[1].color = new Color(221f / 255, 49f / 255, 221f / 255);
        }
        colorGradient.SetKeys(colorKeys, alphaKeys);
        RandomlySpread();   
    }

    void RandomlySpread() {
        // 初始化各粒子位置
        // 随机每个粒子距离中心的半径，同时希望粒子集中在平均半径附近
        for (int i = 0; i < pCount; ++i) {  
            float midRadius = (maxRadius + minRadius) / 2;
            float minRate = Random.Range(1.0f, midRadius / minRadius);
            float maxRate = Random.Range(midRadius / maxRadius, 1.0f);
            float radius = Random.Range(minRadius * minRate, maxRadius * maxRate);
            // 随机每个粒子的角度
            float angle = Random.Range(0.0f, 360.0f);
            float theta = angle / 180 * Mathf.PI;
            // 随机每个粒子的游离起始时间
            float time = Random.Range(0.0f, 360.0f);
            circle[i] = new CirclePosition(radius, angle, time);
            before[i] = radius;
            after[i] = 0.7f * radius;
            if (after[i] < minRadius * 1.1f) {
                after[i] = Random.Range(Random.Range(minRadius, midRadius), (minRadius * 1.1f));
            }
            particleArr[i].position = new Vector3(circle[i].radius * Mathf.Cos(theta), 0f, circle[i].radius * Mathf.Sin(theta));
        }
        particleSys.SetParticles(particleArr, particleArr.Length);
    }

    void Update() {
        
        for (int i = 0; i < pCount; i++) {
            if (ischange) {
                // 开始收缩
                if (circle[i].radius > after[i]) {
                    circle[i].radius -= shrinkSpeed * (circle[i].radius / after[i]) * Time.deltaTime;
                }
            }
            else {
                // 开始还原
                if (circle[i].radius < before[i]) {
                    circle[i].radius += shrinkSpeed * (before[i] / circle[i].radius) * Time.deltaTime;
                }
            }
            if (clockwise) circle[i].angle -= (i % tier + 1) * (speed / circle[i].radius / tier);
            else circle[i].angle += (i % tier + 1) * (speed / circle[i].radius / tier);
            // 保证angle在0~360度
            circle[i].angle = (360.0f + circle[i].angle) % 360.0f; 
            float theta = circle[i].angle / 180 * Mathf.PI;
            // 粒子在半径方向上游离
            circle[i].time += Time.deltaTime;
            circle[i].radius += Mathf.PingPong(circle[i].time, pingPong) - pingPong / 2.0f;

            particleArr[i].startColor = colorGradient.Evaluate(circle[i].angle / 360.0f);
            particleArr[i].position = new Vector3(circle[i].radius * Mathf.Cos(theta), 0f, circle[i].radius * Mathf.Sin(theta));    
        }
        particleSys.SetParticles(particleArr, particleArr.Length);

        // 碰撞检测
        ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "button") ischange = true;
        else ischange = false;
        
    }
}

public class CirclePosition {
    public float radius = 0f, angle = 0f, time = 0f;
    public CirclePosition(float radius, float angle, float time) {
        this.radius = radius;   // 半径
        this.angle = angle;     // 角度
        this.time = time;       // 时间
    }
}
