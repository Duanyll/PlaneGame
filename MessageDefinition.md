# 网络消息内容定义

本文件介绍了 `Socket` 中具体传输的消息内容。

## 用户登录登出

这一部分不是我自己编写的，来自[这个项目](https://github.com/yinyoupoet/chatRoomTest)，关于用户登入登出的部分没有做改变。

| 消息格式 | 含义 |
|---------|------|
| string+`$`|用户请求登陆，string为用户名|
| `#`+string+`#`|登录请求被驳回|
| `server has closed`|服务器主动断开连接|
| `$`|客户端主动断开连接|

## 游戏逻辑

这一部分由我自己定义，每条消息以`*`结束，由`|`分为几个部分

### 服务端到客户端

#### 基本

|Part0|Part1|Part2|说明|
|-|-|-|-|
|CHAT|UserName|Content|聊天
|ULGI|UserName||用户登入
|ULGO|UserName||用户登出
|MBOX|Content||弹出对话框
|SBAR|Content||在SnackBar中显示内容
|SLOG|Content||在日志窗口中显示内容

#### 队伍选取

|Part0|Part1|Part2|说明|
|-|-|-|-|
|GTEM|AvailableTeam||打开队伍选择对话框（AvailableTeam格式：`135`表示1队，3队，5队可选），如果对话框已经开启，就调整参数重新显示
|SGTM|||关闭队伍选择对话框
|TCLR|||清除玩家队伍显示框，准备刷新
|TMIF|UserName|Team|描述哪个玩家属于哪个队

#### 单位摆放

|Part0|Part1|Part2|Part3|说明|
|-|-|-|-|-|
|MPSZ|Width|Height||描述地图的大小
|UCLR|Count|||重置可用单位，可用单位列表大小
|UNIT|Name|Count|PatternGameBoard|新增一种可用单位
|SPUS||||开始放置单位
|UCNT|UnitName|Count||设置指定单位的数量
|GBRD|FullPlayerGameBoard|||整个设置当前棋盘
|UPOS|x|y|GameBoardBlock|单点更新棋盘

#### 攻击阶段

|Part0|Part1|Part2|Part3|Part4|说明|
|-|-|-|-|-|-|
|AGBR|TeamID|PlayerViewGameBoard|||整个设置某个队伍的棋盘
|ASRT|||||开始攻击阶段
|AUGB|TeamID|x|y|GameBoardBlock|单点更新某个队伍的棋盘
|GPNT|Count|TimeOut|||允许开火Count次,TimeOut单位为秒
|SGPT|||||停止开火
|SNOW|UserName|TeamID|||现在轮到谁走
|TLOS|TeamID||||某个队伍输了
|SCOR|UserName|Score|||修改某个玩家的分数

### 客户端向服务端

|Part0|Part1|Part2|Part3|说明|
|-|-|-|-|-|
|CHAT|Content|||聊天
|STEM|TeamID|||加入队伍
|PUTU|Name|x|y|放置单位
|PUCR||||重置游戏版
|FPUS||||完成单位放置阶段
|ATCK|TeamID|x|y|攻击坐标
|REFR||||客户端请求更新UI