package com.owlxgames.oscar.systems;

import com.artemis.Aspect;
import com.artemis.ComponentMapper;
import com.artemis.Entity;
import com.artemis.annotations.Mapper;
import com.artemis.systems.EntityProcessingSystem;
import com.badlogic.gdx.InputProcessor;
import com.badlogic.gdx.graphics.Camera;
import com.badlogic.gdx.math.Vector3;
import com.owlxgames.oscar.Bubble;
import com.owlxgames.oscar.BubbleKind;
import com.owlxgames.oscar.GameConstants;
import com.owlxgames.oscar.components.BoardComponent;
import com.owlxgames.oscar.components.TransformComponent;

import java.util.ArrayList;

public class BubbleSelectionSystem extends EntityProcessingSystem implements InputProcessor {
	@Mapper ComponentMapper<BoardComponent> _boardMapper;
	@Mapper ComponentMapper<TransformComponent> _transformMapper;

	private BoardManagementSystem _boardManagementSystem;
	private Camera _camera;
	private BoardComponent _board;
	private TransformComponent _boardTransform;
	private ArrayList<Bubble> _selectedBubbles;
	
	@SuppressWarnings("unchecked")
	public BubbleSelectionSystem(Camera camera) {
		super(Aspect.getAspectForAll(BoardComponent.class));
		_camera = camera;		
		_selectedBubbles = new ArrayList<Bubble>();
	}

	@Override
	public void initialize() {
		_boardManagementSystem = world.getSystem(BoardManagementSystem.class);
	}
	
	@Override
	public void inserted(Entity e) {
		_board = _boardMapper.get(e);
		_boardTransform = _transformMapper.get(e);
	}

	@Override
	protected void process(Entity e) { }
	
	@Override
	public boolean touchDown(int screenX, int screenY, int pointer, int button) {
		Vector3 touchLocation = new Vector3(screenX, screenY, 0f);
		_camera.unproject(touchLocation);
		
		if (touchIsInGrid(touchLocation)) {
			int col = (int)((touchLocation.x - _boardTransform.position.x) / GameConstants.squareSize);
			int row = (int)((touchLocation.y - _boardTransform.position.y) / GameConstants.squareSize);
			selectBubble(col, row);
			return true;
		}
		return false;
	}
	
	@Override 
	public boolean touchDragged(int screenX, int screenY, int pointer) {
		// TODO Auto-generated method stub
		return false;
	}
	
	private boolean touchIsInGrid(Vector3 touchLocation) {
		return (touchLocation.x > _boardTransform.position.x && touchLocation.x < _boardTransform.position.x + GameConstants.gridWidth) &&
			(touchLocation.y > _boardTransform.position.y && touchLocation.y < _boardTransform.position.y + GameConstants.gridHeight);
	}
	
	private void selectBubble(int column, int row) {
		Bubble bubble = _board.bubbles[column][row];
		if (bubble == null) { return; }
		
		if (bubble.isSelected) {
			_boardManagementSystem.scoreSelectedBubbles();
			
			// TODO Move to BoardManagementSystem
			for (int boardColumn = 0; boardColumn < _board.bubbles.length; boardColumn++) {
				for (int boardRow = 0; boardRow < _board.bubbles[boardColumn].length; boardRow++) {
					if (_board.bubbles[boardColumn][boardRow] != null && _board.bubbles[boardColumn][boardRow].isSelected) {
						_board.bubbles[boardColumn][boardRow] = null;
					}
				}
			}
			_selectedBubbles.clear();
		} else {
			clearSelectedBubbles();
			selectConnectedBubbles(column, row, bubble.kind);
			if (_selectedBubbles.size() <= 1) { 
				clearSelectedBubbles();
			}
		}
	}
	
	private void selectConnectedBubbles(int column, int row, BubbleKind kind) {
		if (column < 0 || column > _board.bubbles.length - 1 || row < 0 || row > _board.bubbles[column].length - 1) {
			return;
		}
		
		Bubble bubble = _board.bubbles[column][row];
		if (bubble == null || bubble.isSelected == true || bubble.kind != kind) {
			return;
		} else {
			bubble.isSelected = true;
			_selectedBubbles.add(bubble);
			selectConnectedBubbles(column - 1, row, kind);
			selectConnectedBubbles(column + 1, row, kind);
			selectConnectedBubbles(column, row - 1, kind);
			selectConnectedBubbles(column, row + 1, kind);
		}		
	}
	
	private void clearSelectedBubbles() {
		for(Bubble bubble: _selectedBubbles) {
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
