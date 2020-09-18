class Boss{
    constructor() {
		this.radius = 35;
		this.velocity = {
			x: (Math.random() - 0.5) * 3,
			y: (Math.random() - 0.5) * 3
		}
		this.changeColour();
		this.positionRandomly();
		this.health = 500;
	}

	draw() {
		context.beginPath();
		context.arc(this.x, this.y, this.radius, 0, 2 * Math.PI);
		context.closePath();
		context.strokeStyle = `rgba(${this.colours[0]}, ${this.colours[1]}, ${this.colours[2]}, 0.75)`;
		context.fillStyle = `rgba(${this.colours[0]}, ${this.colours[1]}, ${this.colours[2]}, 0.25)`;
		context.lineWidth = 3;
		context.stroke();
		context.fill();
	}

	displayStatus() {
		context.save();
		context.font = "12px Arial";
		context.fillStyle = "black";
		context.fillText(`Boss Health: ${this.health.toFixed(0)}`, 10, 140);
		context.restore();
	}

	update(c) {
		this.checkBounds();

        if (this.hitCharacter(c)) {
            this.changeColour();
            if(c.health > 0)
            {
                c.health = c.health-0.8;
            }				
        }

		this.x += this.velocity.x;
		this.y += this.velocity.y;

		this.draw();
		this.displayStatus();
	}

	checkBounds() {
		//check border
		if ((this.x + this.radius) > canvas.width || (this.x - this.radius) < 0) {
			this.velocity.x = -this.velocity.x;
		}

		if ((this.y + this.radius) > canvas.height || (this.y - this.radius) < 0) {
			this.velocity.y = -this.velocity.y;
		}

		//check safe zone horizontal
		if((this.x + this.radius) > (canvas.width-100) && (this.y + this.radius) > 300 && (this.y - this.radius) < 400) {
			this.velocity.x = -this.velocity.x;
		}
		else if((this.x - this.radius) < 100 && (this.y + this.radius) > 300 && (this.y - this.radius) < 400) {
			this.velocity.x = -this.velocity.x;
		}

		//check safe zone verical
		if((this.y + this.radius) > 300 && (((this.x - this.radius) < 100) || ((this.x + this.radius) > (canvas.width-100)))) {
			this.velocity.y = -this.velocity.y;
		}

		if((this.y - this.radius) < 400 && (((this.x - this.radius) < 100) || ((this.x + this.radius) > (canvas.width-100)))) {
			this.velocity.y = -this.velocity.y;
		}
	}

	//logic based from Vikram Singh
	calculateDistance(a) {
		return Math.hypot(this.x - a.position.x, this.y - a.position.y);
	}

	isBulletHit(bullet)
	{
		const distance = this.calculateDistance(bullet);
		if(distance <= (this.radius + bullet.radius) && distance > 0)
			this.health = this.health - 5;
		if(this.health <= 0)
			this.health = 0;
	}

    hitCharacter(c){
		const distance = this.calculateDistance(c);
		return (distance <= (this.radius + c.radius) && distance > 0);
	}

	changeColour() {
		this.colours = [Math.random() * 255, Math.random() * 255, Math.random() * 255];
	}

	positionRandomly() {
		this.x = this.radius + (Math.random() * (canvas.width - (this.radius * 2)));
		this.y = this.radius + (Math.random() * (canvas.height - (this.radius * 2)));
		while(this.x - this.radius < 100 || this.x + this.radius > canvas.width - 100)
		{
			this.x = this.radius + (Math.random() * (canvas.width - (this.radius * 2)));
			this.y = this.radius + (Math.random() * (canvas.height - (this.radius * 2)));
		}
	}
}