# Unity3D游戏一：IMGUI井字棋

### 要求

- 游戏内容： 井字棋
- 技术限制： 仅允许使用 **IMGUI** 构建 UI
- 作业目的：
  - 了解 OnGUI() 事件，提升 debug 能力
  - 提升阅读 API 文档能力

### 实现

- 完整代码传送门->[🚪][https://github.com/Tifinity/Unity3DStudy-master/tree/master/%E7%AC%AC%E4%B8%80%E6%AC%A1%E4%BD%9C%E4%B8%9A%EF%BC%9A%E4%BA%95%E5%AD%97%E6%A3%8B]

- 首先使用IMGUI搭建出游戏界面

  - OnGUI()函数，与Update()一样，只要脚本启用，每一帧都会被调用。

  - GUI.Box()制作背景盒

  - GUI.Label()创建文本框，仅用于显示信息，不能交互。

  - GUI.Button()创建按钮

    ```c#
     if (GUI.Button(new Rect(400, 200, 140, 50), "reset")) {
         //do
     }
    ```

    GUI.Button函数的返回值是0或1，0表示这个按钮没有被点击，1表示被点击，所以这句话的意思是创建按钮并判断该按钮是否被点击。

- 游戏逻辑

  - 检查胜利条件，check()通过返回值不同来表示当前游戏状态。
  
    ```c#
    int check() {
        /*横线获胜*/
        for (int i = 0; i < 3; i++) {
            if (board[i][0] != 0 && board[i][0] == board[i][1] && board[i][1] == board[i][2]) {
                return board[i][0];
            }
        }
        /*纵线获胜*/
        for (int i = 0; i < 3; i++) {
            if (board[0][i] != 0 && board[0][i] == board[1][i] && board[1][i] == board[2][i]) {
                return board[0][i];
            }
        }
        /*斜线获胜*/
        if (board[1][1] != 0 &&
            board[0][0]== board[1][1] && board[2][2] == board[1][1] ||
            board[0][2] == board[1][1] && board[2][0] == board[1][1]) {
            return board[1][1];
        }
        /*对局还没结束*/
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                if (board[i][j] == 0) return 0;
            }
        }
        /*平局*/
        return 3;
    }
    ```
  
  - 先绘制已经下好的棋子，再判断游戏是否结束，若没结束则根据turn的值进行本次点击，更改二维数组的值。
  
    ```c#
    for (int i = 0; i < 3; ++i) {
    	for (int j = 0; j < 3; ++j) {
            if (board[i][j] == 1) {
                GUI.Button(new Rect(m - 150 + i * 100, 200 + j * 100, 100, 100), "O");
            }
            else if (board[i][j] == 2) {
                GUI.Button(new Rect(m - 150 + i * 100, 200 + j * 100, 100, 100), "X");
            }   
            if(GUI.Button(new Rect(m - 150 + i * 100, 200 + j * 100, 100, 100), "")) { 
                if (res == 0) {
                    if (turn == 1) {
                        board[i][j] = turn;
                        turn = 2;
                    }
                    else {
                        board[i][j] = turn;
                        turn = 1;
                    }
                }  
            }
        }
    }
    ```

### 游戏截图

![demo](https://github.com/Tifinity/Unity3DStudy-master/blob/master/第一次作业：井字棋/images/demo.jpg)

### 视频链接





### 参考资料

[Unity3D用户手册中文翻译 by 游戏蛮牛][http://docs.manew.com/Manual/index.htm]



