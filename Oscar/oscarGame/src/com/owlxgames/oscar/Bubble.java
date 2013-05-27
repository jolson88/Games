package com.owlxgames.oscar;

import com.badlogic.gdx.math.Vector2;

public class Bubble {
	public Vector2 offset;
	public BubbleKind kind;
	public boolean isSelected;
	
	public Bubble(Vector2 offset, BubbleKind kind) {
		this.offset = offset;
		this.kind = kind;
		this.isSelected = false;
	}
}
