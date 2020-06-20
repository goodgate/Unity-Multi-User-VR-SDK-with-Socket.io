const http = require('http');
const express = require('express');
const app = express();
const server = http.createServer(app);
server.listen(4567);

var io = require('socket.io')({
    transports: ['websocket']
});
io.attach(server);
var players = [];

io.on('connection', socket => {
    console.log('cnt');
    socket.on('playerEnter',msg=>{
        var player = {
            "id":msg.id,
            "playerIndex":msg.playerIndex,
            "socketID":socket.id
        }
        players.push(player);
        io.emit('playerEnter', {players});
        console.log(players);
    });

    socket.on('disconnect', msg=>{
        const player = players.find(function(item) {return item.socketID === socket.id});
        let idx = players.indexOf(player);
        if (idx > -1) {
            players.splice(idx, 1);
            console.log('id : ' + socket.id);
        }
        console.log(player);
        io.emit('playerExit', player);
    });
    
    socket.on('deadReckoning', msg=>{
        socket.broadcast.emit('deadReckoning', msg);
    });


    //custom Event
    //socket.on('eventName', msg=>{
    //    //do something
    //})
    //
    //
});