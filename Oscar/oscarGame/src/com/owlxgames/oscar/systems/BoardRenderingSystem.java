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
import com.owlxgames.oscar.BubbleKind;
import com.owlxgames.oscar.GameConstants;
import com.owlxgames.oscar.components.BubbleComponent;
import com.owlxgames.oscar.components.TransformComponent;

public class BoardRenderingSystem extends EntityProcessingSystem {
	@Mapper ComponentMapper<TransformComponent> transformMapper;
	@Mapper ComponentMapper<BubbleComponent> bubbleMapper;

	private Camera _camera;
	private SpriteBatch _batch;
	private Texture _grid;
	private ShapeRenderer _shapeRenderer;
	private HashMap<BubbleKind, Color> _kindToColor;
	
	@SuppressWarnings("unchecked")
	public BoardRenderingSystem(Camera camera) {
		super(Aspect.getAspectForAll(TransformComponent.class, BubbleComponent.class));
		
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
		TransformComponent transform = transformMapper.get(e);
		BubbleComponent bubble = bubbleMapper.get(e);
		setRenderingPositionForBubble(transform, bubble);
	}
	
	@Override
	protected void begin() {
		_batch.setProjectionMatrix(_camera.combined);
		_shapeRenderer.setProjectionMatrix(_camera.combined);
		_shapeRenderer.begin(ShapeType.Line);
		_batch.begin();
		_batch.draw(_grid, GameConstants.gridLocation.x, GameConstants.gridLocation.y);
	}
	
	@Override
	protected void process(Entity e) {
		TransformComponent transform = transformMapper.get(e);
		BubbleComponent bubble = bubbleMapper.get(e);
		
		float radius = GameConstants.bubbleSize / 2;
		_shapeRenderer.setColor(_kindToColor.get(bubble.kind));
		_shapeRenderer.circle(transform.position.x + radius, transform.position.y + radius, radius);
		
		if (bubble.isSelected) {
			_shapeRenderer.setColor(1f, 1f, 1f, 1f);
			_shapeRenderer.rect(transform.position.x - 2, transform.position.y - 2, GameConstants.bubbleSize + 4, GameConstants.bubbleSize + 4);
		}
	}
	
	@Override
	protected void end() {
		_batch.end();
		_shapeRenderer.end();
	}
	
	private void setRenderingPositionForBubble(TransformComponent transform, BubbleComponent bubble) {
		transform.position.set(GameConstants.gridLocation.x + calculateOffset(bubble.column), 
				GameConstants.gridLocation.y + calculateOffset(bubble.row));
	}
	
	private int calculateOffset(int square) {
		return (square * GameConstants.squareSize) + ((GameConstants.squareSize - GameConstants.bubbleSize) / 2);
	}
}
