# Unity Multi-User VR SDK with SocketIO

## 적용방법
* 노드 모듈 인스톨
```c
npm install
```
* 유니티 프로젝트를 엽니다.
* 유니티 씬을 엽니다. "JooVRNetwork/SampleScene.unity"   
* 해당 씬을 빌드합니다.   
* node를 실행합니다.
```c
node app.js
```
* 빌드된 파일을 실행하고 Unity 에디터에서 Play
* 연결 상태를 확인합니다.
```c
cnt
[ { id: '', playerIndex: 0, socketID: 'mtxzFjMK8QhJftF_AAAA' } ]
id : mtxzFjMK8QhJftF_AAAA
```

## 커스터마이징
### IP, Port 변경
* app.js 파일을 수정합니다.
```c
const server = http.createServer(app);
//server.listen(4567);
server.listen(4567, '192.168.0.XXX');
```
* Unity Scene의 SocketIO Component Url을 수정합니다.
* unity에서의 socket.io의 자세한 내용은 다음을 참고하십시오.
> Socket.IO for Unity https://assetstore.unity.com/packages/tools/network/socket-io-for-unity-21721

### event 추가
```c
//app.js
io.on('connection', socket => {
    //...
    
    //custom Event
    socket.on('eventName', msg=>{
        //do something
    })
});
```
```c
//Unity
socket.Emit("eventName", JSONObject.Create(jsonStr));
```

## 주의사항
* JooConfigs/NetworkPlayer.json파일의 playerIndex값을 실행하는 클라이언트마다 다르게 설정하십시오.
> ###### (중복값을 가진 클라이언트가 접속할 경우 정상작동하지 않습니다.)
