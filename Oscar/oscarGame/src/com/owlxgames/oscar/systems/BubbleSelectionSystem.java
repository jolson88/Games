package com.owlxgames.oscar.systems;

import com.artemis.ComponentMapper;
import com.artemis.Entity;
import com.artemis.managers.GroupManager;
import com.artemis.systems.VoidEntitySystem;
import com.artemis.utils.*;
import com.badlogic.gdx.InputProcessor;
import com.badlogic.gdx.graphics.Camera;
import com.badlogic.gdx.math.Vector3;
import com.owlxgames.oscar.GameConstants;
import com.owlxgames.oscar.Groups;
import com.owlxgames.oscar.components.BubbleComponent;

public class BubbleSelectionSystem extends VoidEntitySystem implements InputProcessor {
	private ComponentMapper<BubbleComponent> _bubbleMapper;
	private Camera _camera;
	private GroupManager _groupManager;
	
	public BubbleSelectionSystem(Camera camera) {
		_camera = camera;
	}
	
	@Override
	protected void initialize() {
		_bubbleMapper = world.getMapper(BubbleComponent.class);
		_groupManager = world.getManager(GroupManager.class);
	}
	@Override
	protected void processSystem() {
		// TODO Auto-generated method stub
	}
	
	@Override
	public boolean touchDown(int screenX, int screenY, int pointer, int button) {
		Vector3 touchLocation = new Vector3(screenX, screenY, 0f);
		_camera.unproject(touchLocation);
		
		if (touchIsInGrid(touchLocation)) {
			int col = (int)((touchLocation.x - GameConstants.gridLocation.x) / GameConstants.squareSize);
			int row = (int)((touchLocation.y - GameConstants.gridLocation.y) / GameConstants.squareSize);
			selectBubble(col, row);
		}
		
		// TODO Auto-generated method stub
		return false;
	}
	
	@Override 
	public boolean touchDragged(int screenX, int screenY, int pointer) {
		// TODO Auto-generated method stub
		return false;
	}
	
	private boolean touchIsInGrid(Vector3 touchLocation) {
		return (touchLocation.x > GameConstants.gridLocation.x && touchLocation.x < GameConstants.gridLocation.x + GameConstants.gridWidth) &&
			(touchLocation.y > GameConstants.gridLocation.y && touchLocation.y < GameConstants.gridLocation.y + GameConstants.gridHeight);
	}
	
	private void selectBubble(int column, int row) {
		ImmutableBag<Entity> bubbles = _groupManager.getEntities(Groups.bubbles);
		
		Entity e;
		BubbleComponent bubble;
		for (int i = 0; i < bubbles.size(); i++) {
			e = bubbles.get(i);
			bubble = _bubbleMapper.get(e);
			if (bubble.column == column && bubble.row == row) {
				clearSelectedBubbles();
				bubble.isSelected = true;
				break;
			}
		}
	}
	
	private void clearSelectedBubbles() {
		ImmutableBag<Entity> bubbles = _groupManager.getEntities(Groups.bubbles);
		
		Entity e;
		BubbleComponent bubble;
		for (int i = 0; i < bubbles.size(); i++) {
			e = bubbles.get(i);
			bubble = _bubbleMapper.get(e);
			bubble.isSelected = false;
		}
	}
	
	@Override public boolean keyDown(int keycode) { return false; }
	@Override public boolean keyUp(int keycode) { return false; }
	@Override public boolean keyTyped(char character) { return false; }
	@Override public boolean touchUp(int screenX, int screenY, int pointer, int button) { return false; }
	@Override public boolean mouseMoved(int screenX, int screenY) { return false; }
	@Override public boolean scrolled(int amount) { return false; }
}
