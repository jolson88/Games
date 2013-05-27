package com.owlxgames.oscar.systems;

import java.util.Random;

import com.artemis.Aspect;
import com.artemis.ComponentMapper;
import com.artemis.Entity;
import com.artemis.annotations.Mapper;
import com.artemis.systems.EntityProcessingSystem;
import com.badlogic.gdx.math.Vector2;
import com.owlxgames.oscar.Bubble;
import com.owlxgames.oscar.BubbleKind;
import com.owlxgames.oscar.components.BoardComponent;

public class BoardManagementSystem extends EntityProcessingSystem {
	@Mapper ComponentMapper<BoardComponent> _boardMapper;
	
	private Random _random;
	private BoardComponent _board;
	
	@SuppressWarnings("unchecked")
	public BoardManagementSystem() {
		super(Aspect.getAspectForAll(BoardComponent.class));
		_random = new Random();
	}
	
	public void scoreSelectedBubbles() {

	}
	
	@Override
	protected void inserted(Entity e) {
		_board = _boardMapper.get(e);
		initializeBoard();
	}
	
	@Override
	protected void process(Entity e) { }
	
	private void initializeBoard() {
		for(int col = 0; col < _board.bubbles.length; col++) {
			for(int row = 0; row < _board.bubbles[col].length; row++) {
				_board.bubbles[col][row] = new Bubble(new Vector2(0,0), getRandomBubbleKind());
			}
		}
	}
	
	private BubbleKind getRandomBubbleKind() {
		int pick = _random.nextInt(BubbleKind.values().length);
		return BubbleKind.values()[pick];
	}
}
