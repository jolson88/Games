package com.owlxgames.oscar.systems;

import java.util.ArrayList;
import java.util.Random;

import com.artemis.Aspect;
import com.artemis.ComponentMapper;
import com.artemis.Entity;
import com.artemis.annotations.Mapper;
import com.artemis.systems.EntityProcessingSystem;
import com.badlogic.gdx.math.Vector2;
import com.owlxgames.oscar.Bubble;
import com.owlxgames.oscar.BubbleKind;
import com.owlxgames.oscar.BubblesScoredEvent;
import com.owlxgames.oscar.components.BoardComponent;
import com.squareup.otto.Bus;

public class BoardManagementSystem extends EntityProcessingSystem {
	@Mapper ComponentMapper<BoardComponent> _boardMapper;

	private Bus _bus;
	private Random _random;
	private BoardComponent _board;

	@SuppressWarnings("unchecked")
	public BoardManagementSystem(Bus bus) {
		super(Aspect.getAspectForAll(BoardComponent.class));
		_random = new Random();
		_bus = bus;
	}

	public void scoreSelectedBubbles(ArrayList<Bubble> selectedBubbles) {
		_bus.post(new BubblesScoredEvent(selectedBubbles));
		
		for (int column = 0; column < _board.bubbles.length; column++) {
			for (int row = 0; row < _board.bubbles[column].length; row++) {
				if (_board.bubbles[column][row].isSelected) {
					_board.bubbles[column][row].isSelected = false;
					_board.bubbles[column][row].isRemoved = true;
				}
			}
		}
		collapseBubbles();
	}

	@Override
	protected void inserted(Entity e) {
		_board = _boardMapper.get(e);
		initializeBoard();
	}

	@Override
	protected void process(Entity e) {
	}

	private void initializeBoard() {
		for (int col = 0; col < _board.bubbles.length; col++) {
			for (int row = 0; row < _board.bubbles[col].length; row++) {
				_board.bubbles[col][row] = new Bubble(new Vector2(0, 0),
						getRandomBubbleKind());
			}
		}
	}

	private BubbleKind getRandomBubbleKind() {
		int pick = _random.nextInt(BubbleKind.values().length);
		return BubbleKind.values()[pick];
	}

	private void collapseBubbles() {
		collapseRows();
		collapseColumns();
	}

	private void collapseRows() {
		boolean filled = false;
		for (int column = 0; column < _board.bubbles.length; column++) {
			for (int row = 0; row < _board.bubbles[column].length; row++) {
				if (_board.bubbles[column][row].isRemoved) {
					filled = fillWithReplacementBubble(column, row);
					if (!filled) {
						// No bubbles left in this column
						break;
					}
				}
			}
		}		
	}
	
	private void collapseColumns() {
		for (int column = 0; column < _board.bubbles.length; column++) {
			if (_board.bubbles[column][0].isRemoved) {
				for (int columnToSwap = column + 1; columnToSwap < _board.bubbles.length; columnToSwap++) {
					if (!_board.bubbles[columnToSwap][0].isRemoved) {
						// Swap columns
						for (int row = 0; row < _board.bubbles[column].length; row++) {
							swapBubbles(column, row, columnToSwap, row);
						}
						break;
					}
				}
			}
		}
	}
	
	private boolean fillWithReplacementBubble(int column, int row) {
		boolean filled = false;
		for (int replacementRow = row + 1; replacementRow < _board.bubbles[column].length; replacementRow++) {
			if (!_board.bubbles[column][replacementRow].isRemoved) {
				swapBubbles(column, row, column, replacementRow);
				filled = true;
				break;
			}
		}
		return filled;
	}

	private void swapBubbles(int destinationColumn, int destinationRow, int sourceColumn, int sourceRow) {
		boolean destinationIsRemoved = _board.bubbles[destinationColumn][destinationRow].isRemoved;
		BubbleKind destinationKind = _board.bubbles[destinationColumn][destinationRow].kind;
		
		_board.bubbles[destinationColumn][destinationRow].isRemoved = _board.bubbles[sourceColumn][sourceRow].isRemoved;
		_board.bubbles[destinationColumn][destinationRow].kind = _board.bubbles[sourceColumn][sourceRow].kind;
		
		_board.bubbles[sourceColumn][sourceRow].isRemoved = destinationIsRemoved;
		_board.bubbles[sourceColumn][sourceRow].kind = destinationKind;	
	}
}
