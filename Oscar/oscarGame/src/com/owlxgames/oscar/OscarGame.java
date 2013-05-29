package com.owlxgames.oscar;
import com.artemis.*;
import com.artemis.managers.GroupManager;
import com.artemis.managers.TagManager;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.*;
import com.owlxgames.oscar.components.BoardComponent;
import com.owlxgames.oscar.components.GameStateComponent;
import com.owlxgames.oscar.components.TransformComponent;
import com.owlxgames.oscar.systems.*;
import com.squareup.otto.Bus;
import com.squareup.otto.ThreadEnforcer;

public class OscarGame implements ApplicationListener {
	private OrthographicCamera _camera;
	private World _world;
	private Bus _bus;
	
	@Override
	public void create() {	
		GroupManager groupManager = new GroupManager();
		TagManager tagManager = new TagManager();
		
		_bus = new Bus(ThreadEnforcer.ANY);
		
		_camera = new OrthographicCamera();
		_camera.setToOrtho(false, 480, 800);

		_world = new World();	
		_world.setManager(groupManager);
		_world.setManager(tagManager);
		
		Entity boardEntity = _world.createEntity();
		boardEntity.addComponent(new TransformComponent(24, 130));
		boardEntity.addComponent(new BoardComponent(9, 11));
		boardEntity.addToWorld();
		tagManager.register(Tags.board, boardEntity);
		
		Entity stateEntity = _world.createEntity();
		stateEntity.addComponent(new GameStateComponent());
		stateEntity.addToWorld();
		tagManager.register(Tags.gameState, stateEntity);
		
		BubbleSelectionSystem bubbleSelectionSystem = new BubbleSelectionSystem(_camera);
		Gdx.input.setInputProcessor(bubbleSelectionSystem);
		
		_world.setSystem(bubbleSelectionSystem);
		_world.setSystem(new BoardManagementSystem(_bus));
		_world.setSystem(new ScoringSystem(_bus));
		_world.setSystem(new BoardRenderingSystem(_camera));	
		_world.setSystem(new HudRenderingSystem());
		_world.initialize();
	}

	@Override
	public void dispose() {

	}

	@Override
	public void render() {
		_camera.update();
		Gdx.gl.glClearColor(0, 0, 0, 1);
		Gdx.gl.glClear(GL10.GL_COLOR_BUFFER_BIT);

		_world.setDelta(Gdx.graphics.getDeltaTime());
		_world.process();
	}

	@Override
	public void resize(int width, int height) {
	
	}

	@Override
	public void pause() {
	
	}

	@Override
	public void resume() {
	
	}
}
