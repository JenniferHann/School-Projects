class Character {
	constructor(x, y) {
		this.position = new Vector(x, y);
		this.speed = 0;
		this.maxSpeed = 5;
		this.rotation = 0;
		this.friction = 0.95;
		this.radius = 30;
		this.health = 150;
	}

	draw() {
		this.drawCharacter();
		this.displayStatus();
	}

	update() {
		this.checkBounds();
		this.updatePosition();
		this.draw();
		if(this.health <= 0)
		{
			this.health = 0;
		}
	}

	drawCharacter() {
		context.save();
		
		context.translate(this.position.x, this.position.y);
		context.rotate(this.rotation * Math.PI / 180);
		
		context.beginPath();
		context.arc(0, 0, this.radius, 0, 2 * Math.PI);
		context.closePath();
		context.stroke();
		context.restore();
	}

	displayStatus() {
		context.save();
		context.font = "12px Arial";
		context.fillStyle = "black";
		context.fillText(`Position: (${this.position.x.toFixed(0)}, ${this.position.y.toFixed(0)})`, 10, 20);
		context.fillText(`Rotation: ${this.rotation.toFixed(2)}`, 10, 40);
		context.fillText(`Speed: ${this.speed.toFixed(2)}`, 10, 60);
		context.fillText(`Health: ${this.health.toFixed(0)}`, 10, 80);
		context.restore();
	}

	updatePosition() {
		const velocityX = Math.cos(this.rotation * Math.PI / 180) * this.speed;
		const velocityY = Math.sin(this.rotation * Math.PI / 180) * this.speed;
		const velocity = new Vector(velocityX, velocityY);

		this.position.add(velocity);
	}

	checkBounds() {
		if((this.position.x - this.radius) < 0){
			this.position.x = 0 + this.radius;
		}

		if ((this.position.x + this.radius) > canvas.width){
			this.position.x = canvas.width - this.radius;
		}

		if ((this.position.y + this.radius) > canvas.height){
			this.position.y = 0 + this.radius;
		}

		if((this.position.y - this.radius) < 0){
			this.position.y = canvas.height - this.radius;
		}
    }
    
    goForward()
    {
        if (this.isGoingFullSpeed()) {
			this.speed = this.maxSpeed;
		}
		else {
			this.speed += 0.1;
		}
    }

    goBackward()
    {
        if (this.isGoingFullSpeed()) {
			this.speed = -this.maxSpeed;
		}
		else {
			this.speed -= 0.1;
		}
	}
	
	stop() {
		this.speed = 0;
	}

	goUp() {
		this.rotation = 270;
	}

	goDown() {
        this.rotation = 90;
    }
    
    goRight() {
        this.rotation = 0;
    }

    goLeft() {
        this.rotation = 180;
    }

	goDiagonalUpRight()
	{
		this.rotation = 315;
	}

	goDiagonalUpLeft()
	{
		this.rotation = 225;
	}

	goDiagonalDownRight()
	{
		this.rotation = 45;
	}

	goDiagonalDownLeft()
	{
		this.rotation = 135;
	}

	isGoingFullSpeed() {
		return this.speed >= this.maxSpeed || this.speed <= -this.maxSpeed;
	}
}
