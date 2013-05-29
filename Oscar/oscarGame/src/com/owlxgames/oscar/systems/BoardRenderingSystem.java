package com.owlxgames.oscar.systems;

import java.util.HashMap;

import com.artemis.Aspect;
import com.artemis.ComponentMapper;
import com.artemis.Entity;
import com.artemis.annotations.Mapper;
import com.artemis.systems.EntityProcessingSystem;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Camera;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer.ShapeType;
import com.badlogic.gdx.math.Vector2;
import com.owlxgames.oscar.Bubble;
import com.owlxgames.oscar.BubbleKind;
import com.owlxgames.oscar.GameConstants;
import com.owlxgames.oscar.components.BoardComponent;
import com.owlxgames.oscar.components.TransformComponent;

public class BoardRenderingSystem extends EntityProcessingSystem {
	@Mapper ComponentMapper<TransformComponent> transformMapper;
	@Mapper ComponentMapper<BoardComponent> boardMapper;

	private Camera _camera;
	private SpriteBatch _batch;
	private Texture _grid;
	private ShapeRenderer _shapeRenderer;
	private HashMap<BubbleKind, Color> _kindToColor;
	
	@SuppressWarnings("unchecked")
	public BoardRenderingSystem(Camera camera) {
		super(Aspect.getAspectForAll(TransformComponent.class, BoardComponent.class));
		
		_camera = camera;
		_kindToColor = new HashMap<BubbleKind, Color>();
		_kindToColor.put(BubbleKind.Red, new Color(1f, 0f, 0f, 1f));
		_kindToColor.put(BubbleKind.Green, new Color(0f, 1f, 0f, 1f));
		_kindToColor.put(BubbleKind.Blue, new Color(0f, 0f, 1f, 1f));
		_kindToColor.put(BubbleKind.Orange, new Color(1f, 0.6f, 0f, 1f));
		_kindToColor.put(BubbleKind.Purple, new Color(0.6f, 0f, 1f, 1f));
	}
	
	@Override
	protected void initialize() {
		_batch = new SpriteBatch();
		_shapeRenderer = new ShapeRenderer();
		
		_grid = new Texture(Gdx.files.internal("sprites\\grid.png"));
	}
	
	@Override
	protected void inserted(Entity e) {
		BoardComponent board = boardMapper.get(e);
		setOffsetsForBubbles(board);
	}

	@Override
	protected void process(Entity e) {
		TransformComponent transform = transformMapper.get(e);
		BoardComponent board = boardMapper.get(e);
		
		_batch.setProjectionMatrix(_camera.combined);
		_shapeRenderer.setProjectionMatrix(_camera.combined);
		_shapeRenderer.begin(ShapeType.Line);
		_batch.begin();
		_batch.draw(_grid, transform.position.x, transform.position.y);

		Bubble bubble;
		float radius = GameConstants.bubbleSize / 2;		
		for(int col = 0; col < board.columnCount; col++) {
			for(int row = 0; row < board.rowCount; row++) {
				if (!board.bubbles[col][row].isRemoved) {
					bubble = board.bubbles[col][row];
					
					_shapeRenderer.setColor(_kindToColor.get(bubble.kind));
					_shapeRenderer.circle(transform.position.x + bubble.offset.x + radius, 
							transform.position.y + bubble.offset.y + radius, radius);
					
					if (bubble.isSelected) {
						_shapeRenderer.setColor(1f, 1f, 1f, 1f);
						_shapeRenderer.rect(transform.position.x + bubble.offset.x - 2, 
								transform.position.y + bubble.offset.y - 2, 
								GameConstants.bubbleSize + 4, 
								GameConstants.bubbleSize + 4);
					}
				}
			}
		}
		
		_batch.end();
		_shapeRenderer.end();
	}

	private void setOffsetsForBubbles(BoardComponent board) {
		for(int col = 0; col < board.columnCount; col++) {
			for(int row = 0; row < board.rowCount; row++) {
				board.bubbles[col][row].offset = getRenderingOffsetForBubble(col, row);
			}
		}
	}
	
	private Vector2 getRenderingOffsetForBubble(int column, int row) {
		return new Vector2(calculateOffset(column), calculateOffset(row));
	}
	
	private int calculateOffset(int square) {
		return (square * GameConstants.squareSize) + ((GameConstants.squareSize - GameConstants.bubbleSize) / 2);
	}
}
