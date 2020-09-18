//Get the canvas
let canvas = document.querySelector('canvas');
let context = canvas.getContext('2d');
canvas.width = 900;
canvas.height = 700;

//Music and sound
let pop = new Audio('pop.mp3');
let missionImpossible = new Audio('Mission_Impossible_Theme.mp3');
let jamesBond = new Audio('007_James_Bond_Theme.mp3');

//Avatars
let character = new Character(50, 350); //the user's player
let boss = new Boss(); //target to kill
const enemies = []; //must dodge


//Keyboard input
let keys = {};
canvas.addEventListener('keydown', event => {
    keys[event.key] = true;
});

canvas.addEventListener('keyup', event => {
    keys[event.key] = false;
});

//Bullets 
let trigger =  {fire: false}; //user did not left click to shoot
let ammo = [];
let maxBullets = 5;
let isShot = [];

for(let i = 0; i < maxBullets; i++) //initial bool to allow a shot
{
    isShot[i] = false;
}

function reloadGun() 
{
    for(let i = 0; i < maxBullets; i++)
    {
        ammo[i] = new Bullet(character.position.x, character.position.y);
    }
}
reloadGun(); 

function shootBullet(event)
{
    if(trigger.fire == false)
    {
        pop.play();
        trigger.fire = true;
        if(ammo.length != 0)
            ammo[ammo.length-1].shoot(character.position.x, character.position.y, event.clientX, event.clientY);
    }
}

function reload(event) //reload the gun when spacebar is press
{
    if(event.keyCode == 32)
    {
        trigger.fire = false; //reset
        while(ammo.length != 0) //remove any leftover bullets
        {
            ammo.pop();
        }
        reloadGun(); 
    }
    
}

canvas.addEventListener('click', shootBullet); //character shot
canvas.addEventListener('keydown', reload); //reload gun

//safe zone for character box
function safeZone()
{
    context.fillStyle = "black";
    context.beginPath();
    context.rect(0,300,100,100);
    context.rect(800, 300, 100, 100);
    context.closePath();
    context.stroke();
}

//bool for each stage
let mission1 = false;
let mission2 = false;
let mission3 = false;

//Animate the game
let start; //variable to request animation frame
function animate()
{
    missionImpossible.play();
    missionImpossible.loop = true;
    start = requestAnimationFrame(animate);
    context.clearRect(0,0,canvas.width, canvas.height);

    //character's movement
    if(keys.ArrowUp || keys.w)
    {
        character.goUp();
        character.goForward();
    }
    else if(keys.ArrowDown || keys.s)
    {
        character.goDown();
        character.goForward();
    }
    else if(keys.ArrowLeft || keys.a)
    {
        character.goLeft();
        character.goForward();
    }
    else if(keys.ArrowRight || keys.d)
    {
        character.goRight();
        character.goForward();
    }
    else
    {
        character.stop();
    }
    
    if((keys.ArrowUp && keys.ArrowRight) || (keys.w && keys.d))
    {
        character.goDiagonalUpRight();
    }
    else if((keys.ArrowUp && keys.ArrowLeft) || (keys.w && keys.a))
    {
        character.goDiagonalUpLeft();
    }
    else if((keys.ArrowDown && keys.ArrowRight) || (keys.s && keys.d))
    {
        character.goDiagonalDownRight();
    }
    else if((keys.ArrowDown && keys.ArrowLeft) || (keys.s && keys.a))
    {
        character.goDiagonalDownLeft();
    }

    character.update();

    //enemies movement
    enemies.map(enemy => enemy.update(enemies, character));

    //draw safe zone for character
    safeZone();
    
    if(mission3 == true)
    {
        //show amount of bullets left to shoot 
        displayAmmo();
    
        if(boss.health != 0)
        {

            //boss movement
            boss.update(character);
        }
        
        //a bullet have been shot
        if(trigger.fire == true && ammo.length != 0) 
        {
            boss.isBulletHit(ammo[ammo.length-1]);
            ammo[ammo.length-1].update(ammo, trigger);
        }
        
        //check for the ending of the game
        if(character.health <= 0)
        {
            cancelAnimationFrame(start);
            missionFail();
        }

        if((character.position.x - character.radius > canvas.width-100 && character.position.y - character.radius > 300 && character.position.y + character.radius < 400) && boss.health <= 0)
        {
            cancelAnimationFrame(start);
            finishGame();
        }
    }

    if(mission1 == true || mission2 == true) 
    {
        if(character.health <= 0)
        {
            cancelAnimationFrame(start);
            missionFail();
        }
        if(character.position.x - character.radius > canvas.width-100 && character.position.y - character.radius > 300 && character.position.y + character.radius < 400)
        {
            cancelAnimationFrame(start);
            missionFinish();
        }
    }
}

//make sure the user is ready to play
function Ready()
{
    let width = 400, height = 300, x = 245, y = 200;
    let xButton = 130, yButton = 200, widthButton = 150, heightButton = 50;
    context.save();
    context.translate(x, y);
    context.fillStyle = "#272727";
    context.fillRect(0,0,width, height);
    context.fillStyle = "white";
    context.font = "40px Arial";
    context.fillText("ARE YOU READY?", 25, 60);
    context.font = "20px Arial";
    context.fillText("Agent Circle, ", 25, 90);
    context.fillText("You have arrive at the enemies base.", 25, 120);
    context.fillText("You now need to infiltrate the base", 25, 140);
    context.fillText("Dodge all the enemies and head towards", 25, 160);
    context.fillText("the safe zone.", 25, 180);
    context.fillStyle = "#B9B9B9";
    context.fillRect(xButton, yButton, widthButton, heightButton);
    context.fillStyle = "black";
    context.font = "30px Arial";
    context.fillText("START", 160, 235);
    context.restore();

    function startGame(event) //user is ready to start playing
    {
        if(event.clientX < 530 && event.clientX > 380 && event.clientY < 560 && event.clientY > 410)
        {
            canvas.removeEventListener("click", startGame);
            animate();
        }
    }

    canvas.addEventListener('click', startGame);
}

//show amount of bullets left 
function displayAmmo() {
    context.save();
    context.font = "12px Arial";
    context.fillStyle = "black";
    context.fillText(`Bullets: (${ammo.length})`, 10, 100);
    if(ammo.length == 0)
    {
        context.fillText(`Press spacebar to reload`, 10, 120);
    }
    context.restore();
}

//reset the avatars and start the game again
function replay(sameLevel)
{
    if(sameLevel == true)
        character.health = 150;
    character.position.x = 50;
    character.position.y = 350;
    boss.health = 500;
    reloadGun();
    animate();
}


//menu after game is completed
let xMenuFinishMission = 245, yMenuFinishMission = 200;
let xButReplay = 45, yButReplay = 150, widthReplay = 150, heightReplay = 50;
let xButExit = 200, yButExit = yButReplay, widthExit = widthReplay, heightExit = heightReplay;

function missionFail()
{
    let width = 400, height = 300;
    context.save();
    context.translate(xMenuFinishMission, yMenuFinishMission);
    context.fillStyle = "#272727";
    context.fillRect(0,0,width, height);
    context.fillStyle = "white";
    context.font = "50px Arial";
    context.fillText("GAME OVER", 45, 80);
    context.fillStyle = "#B9B9B9";
    context.fillRect(xButReplay, yButReplay, widthReplay, heightReplay);
    context.fillStyle = "#B9B9B9";
    context.fillRect(xButExit, yButExit, widthExit, heightExit);
    context.fillStyle = "black";
    context.font = "30px Arial";
    context.fillText("Replay", 70, 185);
    context.fillText("Exit", 250, 185);
    context.restore();
    
    canvas.addEventListener('click', menuAfterMissionF);
    
    function menuAfterMissionF(event)
    {
        //replay of the game
        if(event.clientX < 445 && event.clientX > 299 && event.clientY < 405 && event.clientY > 355)
        {
            canvas.removeEventListener("click", menuAfterMissionF);
            let sameLevel = true;
            replay(sameLevel);
        }
        //end the game
        if(event.clientX < 600 && event.clientX > 450 && event.clientY < 405 && event.clientY > 355)
        {
            canvas.removeEventListener("click", menuAfterMissionF);
            finishGame();
        }
    }
}
//player pass the level menu
function missionFinish()
{
    let width = 400, height = 300;
    context.save();
    context.translate(xMenuFinishMission, yMenuFinishMission);
    context.fillStyle = "#272727";
    context.fillRect(0,0,width, height);
    context.fillStyle = "white";
    context.font = "45px Arial";
    context.fillText("Mission Complete", 15, 80);
    context.fillStyle = "#B9B9B9";
    context.fillRect(xButReplay, yButReplay, widthReplay, heightReplay);
    context.fillStyle = "#B9B9B9";
    context.fillRect(xButExit, yButExit, widthExit, heightExit);
    context.fillStyle = "black";
    context.font = "30px Arial";
    context.fillText("Next", 90, 185);
    context.fillText("Exit", 250, 185);
    context.restore();
    
    canvas.addEventListener('click', menuAfterMissionP);
    
    function menuAfterMissionP(event)
    {
        //replay of the game
        if(event.clientX < 445 && event.clientX > 299 && event.clientY < 405 && event.clientY > 355)
        {
            canvas.removeEventListener("click", menuAfterMissionP);
            if(mission1 == true && mission2 == false)
            {
                stage2();
            }
            else if(mission2 == true && mission1 == true)
            {
                stage3();
            }
        }
        //end the game
        if(event.clientX < 600 && event.clientX > 450 && event.clientY < 405 && event.clientY > 355)
        {
            canvas.removeEventListener("click", menuAfterMissionP);
            finishGame();
        }
    }
}

function finishGame()
{
    missionImpossible.pause();
    jamesBond.play();
    jamesBond.loop = true;
    context.clearRect(0,0,canvas.width, canvas.height);
    context.fillStyle = "black";
    context.font = "bolder 50px Arial";
    context.fillText("See You Next Time Agent Circle", 65, 180);
    let agent = 'agent-exit.jpg';
    let img = new Image();
    img.src = agent;
    img.onload = function(){
        context.drawImage(img, 230, 300);
    }
}

//play game without selecting browser
canvas.focus();

//create enemies
function createEnemies(n)
{
    for(let i = 0; i<n ; i++) //create the enemies
    {
        const newEnemy = new Enemies();
        //make sure no enemies spawn on top of each other
        while (enemies.some(enemy => newEnemy.isColliding(enemy))) {
            newEnemy.positionRandomly();
        }
        //make sure no enemies spawn in safe zone
        while(newEnemy.x - newEnemy.radius < 100 || newEnemy.x + newEnemy.radius > canvas.width - 100)
        {
            newEnemy.positionRandomly();
        }    
        enemies.push(newEnemy);
    }
}
//reset number of enemies
function clearEnemies()
{
    while(enemies.length != 0)
    {
        enemies.pop();
    }
}

//mission 1
function stage1()
{
    mission1 = true;
    let numberEnemies = 20;
    createEnemies(numberEnemies);
    Ready();
}
stage1(); //start the game

//mission 2
function stage2()
{
    mission2 = true;
    clearEnemies();
    let numberEnemies = 40;
    createEnemies(numberEnemies);
    let sameLevel = false;

    let info = [];
    info[0] = "Congratulation Agent Circle!";
    info[1] = "You made it inside the enemies's base. ";
    info[2] = "It seems that the enemies have ";
    info[3] = "reinforced security tonight. Dodge all";
    info[4] = "the underlings and head to the safe";
    info[5] = "zone.";

    missionInfo(info);

    function startGameS2(event) //user is ready to start playing
    {
        if(event.clientX < 530 && event.clientX > 380 && event.clientY < 475 && event.clientY > 425)
        {
            canvas.removeEventListener("click", startGameS2);
            replay(sameLevel);
        }
    }

    canvas.addEventListener('click', startGameS2);
}

//mission 3
function stage3()
{
    mission1 = false;
    mission2 = false;
    mission3 = true;
    let sameLevel = false;
    clearEnemies();
    let numberEnemies = 30;
    createEnemies(numberEnemies);
    let info = [];
    info[0] = "Agent Circle, you did it again!"
    info[1] = "The target is now insight, but is still ";
    info[2] = "heavily guarded by bodyguards. Kill the ";
    info[3] = "boss by shooting while dodging the";
    info[4] = "underlings. When done, go to safe zone.";
    info[5] = "Good luck."
    missionInfo(info);

    function startGameS3(event) //user is ready to start playing
    {
        if(event.clientX < 530 && event.clientX > 380 && event.clientY < 475 && event.clientY > 425)
        {
            canvas.removeEventListener("click", startGameS3);
            replay(sameLevel);
        }
    }

    canvas.addEventListener('click', startGameS3);
}

//instruction for each mission
function missionInfo(info)
{
    let width = 400, height = 300, x = 245, y = 200;
    let xButton = 130, yButton = 220, widthButton = 150, heightButton = 50;
    context.save();
    context.translate(x, y);
    context.fillStyle = "#272727";
    context.fillRect(0,0,width, height);
    context.fillStyle = "white";
    context.font = "40px Arial";
    context.fillText("ARE YOU READY?", 25, 60);
    context.font = "20px Arial";
    context.fillText(info[0], 25, 95);
    context.fillText(info[1], 25, 125);
    context.fillText(info[2], 25, 145);
    context.fillText(info[3], 25, 165);
    context.fillText(info[4], 25, 185);
    context.fillText(info[5], 25, 205);
    context.fillStyle = "#B9B9B9";
    context.fillRect(xButton, yButton, widthButton, heightButton);
    context.fillStyle = "black";
    context.font = "30px Arial";
    context.fillText("START", 160, 255);
    context.restore();   
}
