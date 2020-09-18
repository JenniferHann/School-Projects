class Bullet {
    constructor(x, y) {
		this.position = new Vector(x, y);
		this.speed = 0;
		this.maxSpeed = 5;
		this.rotation = 0;
		this.radius = 10;
	}

	update(ammo, trigger) {
		this.checkBounds(ammo, trigger);
		this.goForward();
		this.updatePosition();
		this.drawBullet();
	}

	drawBullet() {
		context.save();
		
		context.translate(this.position.x, this.position.y);
		context.rotate(this.rotation * Math.PI / 180);
		
        context.beginPath();
        context.fillStyle = "black";
		context.arc(0, 0, this.radius, 0, 2 * Math.PI);
		context.closePath();
		context.stroke();
		context.restore();
	}

	updatePosition() {
		const velocityX = Math.cos(this.rotation * Math.PI / 180) * this.speed;
		const velocityY = Math.sin(this.rotation * Math.PI / 180) * this.speed;
		const velocity = new Vector(velocityX, velocityY);

		this.position.add(velocity);
	}

	checkBounds(ammo, pulltrigger) {
		//left border
		if((this.position.x - this.radius) < 0){
			ammo.splice(0,1); //remove bullet
			if(pulltrigger.fire == true) //ready to shoot
				pulltrigger.fire = false;

		}
		//right border
		if ((this.position.x + this.radius) > canvas.width){
			ammo.splice(0,1);
			if(pulltrigger.fire == true)
				pulltrigger.fire = false;
		}
		//bottom border
		if ((this.position.y + this.radius) > canvas.height){
			ammo.splice(0,1);
			if(pulltrigger.fire == true)
				pulltrigger.fire = false;
		}
		//top border
		if((this.position.y - this.radius) < 0){
			ammo.splice(0,1);
			if(pulltrigger.fire == true)
				pulltrigger.fire = false;
		}

	}
    
    shoot(cx, cy, mx, my)
    {
		this.position.x = cx;
		this.position.y = cy;

        let angle, x, y;

		x = mx - cx;
		y = my - cy;
		
		angle = Math.atan2(y,x);
		this.rotation = angle*180/Math.PI;
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

	//logic from Vikram Singh
	isGoingFullSpeed() {
		return this.speed >= this.maxSpeed || this.speed <= -this.maxSpeed;
	}
}