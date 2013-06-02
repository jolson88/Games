package com.owlxgames.oscar.systems;

import com.artemis.Aspect;
import com.artemis.ComponentMapper;
import com.artemis.Entity;
import com.artemis.annotations.Mapper;
import com.artemis.systems.EntityProcessingSystem;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Camera;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.owlxgames.oscar.components.GameStateComponent;

public class HudRenderingSystem extends EntityProcessingSystem {
	@Mapper ComponentMapper<GameStateComponent> _gameStateMapper;
	
	private Camera _camera;
	private SpriteBatch _batch;
	private BitmapFont _headerFont;
	
	@SuppressWarnings("unchecked")
	public HudRenderingSystem(Camera camera) {
		super(Aspect.getAspectForAll(GameStateComponent.class));
		_camera = camera;
	}
	
	@Override
	protected void initialize() {
		_batch = new SpriteBatch();
		_headerFont = new BitmapFont(Gdx.files.internal("fonts/header.fnt"), Gdx.files.internal("fonts/header.png"), false);
	}
	
	@Override
	protected void process(Entity e) {
		GameStateComponent gameState = _gameStateMapper.get(e);
		
		_batch.setProjectionMatrix(_camera.combined);
		_batch.begin();
		_headerFont.draw(_batch, String.valueOf(gameState.score), 12, 1170);
		_headerFont.draw(_batch, "L" + String.valueOf(gameState.level)+ ": " + String.valueOf(gameState.targetScore), 12, 1100);
		_batch.end();
	}

}
