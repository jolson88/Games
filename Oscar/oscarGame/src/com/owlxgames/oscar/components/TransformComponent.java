package com.owlxgames.oscar.components;

import com.artemis.Component;
import com.badlogic.gdx.math.Vector2;

public class TransformComponent extends Component {
	public Vector2 position;
	
	public TransformComponent() {
		position = Vector2.Zero;
	}
	
	public TransformComponent(float x, float y) {
		position = new Vector2(x, y);
	}
}
