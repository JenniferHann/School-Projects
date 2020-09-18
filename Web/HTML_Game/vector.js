class Vector {
	constructor(x, y) {
		this.x = x;
		this.y = y;
	}

	add(vector) {
		this.x += vector.x;
		this.y += vector.y;
	}

	length() {
		return Math.hypot(this.x, this.y);
	}

	direction() {
		return Math.atan2(this.y, this.x);
	}
}
