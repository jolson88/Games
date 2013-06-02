package com.owlxgames.oscar.systems;

import java.util.HashMap;

import com.artemis.Aspect;
import com.artemis.ComponentMapper;
import com.artemis.Entity;
import com.artemis.annotations.Mapper;
import com.artemis.systems.EntityProcessingSystem;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Camera;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer.ShapeType;
import com.owlxgames.oscar.BubbleKind;
import com.owlxgames.oscar.GameConstants;
import com.owlxgames.oscar.components.BubbleComponent;
import com.owlxgames.oscar.components.TransformComponent;

public class BubbleRenderingSystem extends EntityProcessingSystem {
	@Mapper ComponentMapper<TransformComponent> transformMapper;
	@Mapper ComponentMapper<BubbleComponent> bubbleMapper;

	private Camera _camera;
	private SpriteBatch _batch;
	private Texture _grid;
	private ShapeRenderer _shapeRenderer;
	private HashMap<BubbleKind, Texture> _kindToColor;
	
	@SuppressWarnings("unchecked")
	public BubbleRenderingSystem(Camera camera) {
		super(Aspect.getAspectForAll(TransformComponent.class, BubbleComponent.class));
		
		_camera = camera;
		_kindToColor = new HashMap<BubbleKind, Texture>();
	}
	
	@Override
	protected void initialize() {
		_batch = new SpriteBatch();
		_shapeRenderer = new ShapeRenderer();
		
		_grid = new Texture(Gdx.files.internal("sprites\\grid.png"));
		_kindToColor.put(BubbleKind.Red, new Texture(Gdx.files.internal("sprites\\bubble-red.png")));
		_kindToColor.put(BubbleKind.Green, new Texture(Gdx.files.internal("sprites\\bubble-green.png")));
		_kindToColor.put(BubbleKind.Blue, new Texture(Gdx.files.internal("sprites\\bubble-blue.png")));
		_kindToColor.put(BubbleKind.Orange, new Texture(Gdx.files.internal("sprites\\bubble-orange.png")));
		_kindToColor.put(BubbleKind.Purple, new Texture(Gdx.files.internal("sprites\\bubble-purple.png")));
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

		if (!bubble.isRemoved) {
			_batch.draw(_kindToColor.get(bubble.kind), transform.getPosition().x, transform.getPosition().y);
			
			if (bubble.isSelected) {
				_shapeRenderer.setColor(1f, 1f, 1f, 1f);
				_shapeRenderer.rect(transform.getPosition().x - 2, 
						transform.getPosition().y - 2, 
						GameConstants.bubbleSize + 4, 
						GameConstants.bubbleSize + 4);
			}
		}
	}

	@Override
	protected void end() {
		_batch.end();
		_shapeRenderer.end();		
	}
}
