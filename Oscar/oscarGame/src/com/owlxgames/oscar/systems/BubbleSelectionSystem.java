package com.owlxgames.oscar.systems;

import com.artemis.Aspect;
import com.artemis.ComponentMapper;
import com.artemis.Entity;
import com.artemis.annotations.Mapper;
import com.artemis.systems.EntityProcessingSystem;
import com.badlogic.gdx.InputProcessor;
import com.badlogic.gdx.graphics.Camera;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.math.Vector3;
import com.owlxgames.oscar.BubbleKind;
import com.owlxgames.oscar.BubblesPoppedEvent;
import com.owlxgames.oscar.components.BubbleComponent;
import com.owlxgames.oscar.components.TransformComponent;
import com.squareup.otto.Bus;

import java.util.ArrayList;


public class BubbleSelectionSystem extends EntityProcessingSystem implements InputProcessor {
	@Mapper ComponentMapper<BubbleComponent> _bubbleMapper;
	@Mapper ComponentMapper<TransformComponent> _transformMapper;

	private Bus _bus;
	private Camera _camera;
	private ArrayList<Entity> _bubbles;
	private ArrayList<BubbleComponent> _selectedBubbles;
	
	@SuppressWarnings("unchecked")
	public BubbleSelectionSystem(Camera camera, Bus bus) {
		super(Aspect.getAspectForAll(TransformComponent.class, BubbleComponent.class));
		_camera = camera;	
		
		_bubbles = new ArrayList<Entity>();
		_selectedBubbles = new ArrayList<BubbleComponent>();
		_bus = bus;
	}
	
	@Override
	public void inserted(Entity e) {
		_bubbles.add(e);
	}

	@Override
	protected void process(Entity e) { }
	
	@Override
	public boolean touchDown(int screenX, int screenY, int pointer, int button) {
		Vector3 touchLocation = new Vector3(screenX, screenY, 0f);
		_camera.unproject(touchLocation);
		
		boolean bubbleSelected = selectBubble(new Vector2(touchLocation.x, touchLocation.y));
		return bubbleSelected;
	}
	
	@Override 
	public boolean touchDragged(int screenX, int screenY, int pointer) {
		// TODO Auto-generated method stub
		return false;
	}
	
	private boolean selectBubble(Vector2 touchLocation) {
		BubbleComponent bubble;
		TransformComponent transform;
		for(Entity e: _bubbles) {
			bubble = _bubbleMapper.get(e);
			transform = _transformMapper.get(e);
			
			if (transform.getBoundingBox().contains(touchLocation)) {
				if (bubble.isSelected) {
					popBubbles();
					_bus.post(new BubblesPoppedEvent(_selectedBubbles));
					clearSelectedBubbles();
					return true;
				} else {
					clearSelectedBubbles();
					selectConnectedBubbles(bubble, bubble.kind);
					if (_selectedBubbles.size() < 2) {
						clearSelectedBubbles();
					}
					return true;
				}
			}
		}
		return false;
	}
	
	private void selectConnectedBubbles(BubbleComponent bubble, BubbleKind selectedKind) {
		if (!bubble.isRemoved && !bubble.isSelected && bubble.kind == selectedKind) {
			bubble.isSelected = true;
			_selectedBubbles.add(bubble);
			
			if (bubble.leftBubble != null) selectConnectedBubbles(bubble.leftBubble, selectedKind);
			if (bubble.rightBubble != null) selectConnectedBubbles(bubble.rightBubble, selectedKind);
			if (bubble.aboveBubble != null) selectConnectedBubbles(bubble.aboveBubble, selectedKind);
			if (bubble.belowBubble != null) selectConnectedBubbles(bubble.belowBubble, selectedKind);
		}
	}
	
	private void popBubbles() {
		for(BubbleComponent bubble: _selectedBubbles) {
			bubble.isRemoved = true;
		}
	}
	
	private void clearSelectedBubbles() {
		for(BubbleComponent bubble: _selectedBubbles) {
			bubble.isSelected = false;
		}
		_selectedBubbles.clear();
	}
	
	@Override public boolean keyDown(int keycode) { return false; }
	@Override public boolean keyUp(int keycode) { return false; }
	@Override public boolean keyTyped(char character) { return false; }
	@Override public boolean touchUp(int screenX, int screenY, int pointer, int button) { return false; }
	@Override public boolean mouseMoved(int screenX, int screenY) { return false; }
	@Override public boolean scrolled(int amount) { return false; }
}
