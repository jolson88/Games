package com.owlxgames.oscar.components;

import com.artemis.Component;
import com.badlogic.gdx.math.Rectangle;
import com.badlogic.gdx.math.Vector2;

public class TransformComponent extends Component {
	private Vector2 position;
	private float width;
	private float height;
	private Rectangle boundingBox;
	
	public Vector2 getPosition() {
		return this.position;
	}
	
	public void setPosition(Vector2 newPosition) {
		this.position = newPosition;
		buildBoundingBox();
	}
	
	public float getWidth() {
		return this.width;
	}
	
	public void setWidth(float newWidth) {
		this.width = newWidth;
		buildBoundingBox();
	}
	
	public float getHeight() {
		return this.height;
	}
	
	public void setHeight(float newHeight) {
		this.height = newHeight;
		buildBoundingBox();
	}
	
	public Rectangle getBoundingBox() {
		return this.boundingBox;
	}
	
	public TransformComponent() {
		position = Vector2.Zero;
	}
	
	public TransformComponent(float x, float y) {
		position = new Vector2(x, y);
		height = 0;
		width = 0;
		buildBoundingBox();
	}
	
	public TransformComponent(float x, float y, float width, float height) {
		position = new Vector2(x, y);
		this.width = width;
		this.height = height;
		buildBoundingBox();
	}
	
	public TransformComponent(Vector2 position) {
		this.position = position;
		this.width = 0;
		this.height = 0;
		buildBoundingBox();
	}
	
	public TransformComponent(Vector2 position, float width, float height) {
		this.position = position;
		this.width = width;
		this.height = height;
		buildBoundingBox();
	}
	
	private void buildBoundingBox() {
		this.boundingBox = new Rectangle(position.x, position.y, width, height);
	}
}
