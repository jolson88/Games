package com.owlxgames.oscar;

import com.badlogic.gdx.math.Vector2;

public class Bubble {
	public Vector2 offset;
	public BubbleKind kind;
	public boolean isSelected;
	public boolean isRemoved;
	
	public Bubble(Vector2 offset, BubbleKind kind) {
		this.offset = offset;
		this.kind = kind;
		this.isSelected = false;
		this.isRemoved = false;
	}
	
	public Bubble(Vector2 offset, BubbleKind kind, boolean isSelected, boolean isRemoved) {
		this.offset = offset;
		this.kind = kind;
		this.isSelected = isSelected;
		this.isRemoved = isRemoved;		
	}
	
	public Bubble cpy() {
		return new Bubble(this.offset, this.kind, this.isSelected, this.isRemoved);
	}
}
