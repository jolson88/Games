package com.owlxgames.oscar;

import com.artemis.*;
import com.artemis.managers.GroupManager;
import com.artemis.managers.TagManager;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.*;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.math.Vector2;
import com.owlxgames.oscar.components.BubbleComponent;
import com.owlxgames.oscar.components.GameStateComponent;
import com.owlxgames.oscar.components.TransformComponent;
import com.owlxgames.oscar.systems.*;
import com.squareup.otto.Bus;
import com.squareup.otto.ThreadEnforcer;

public class OscarGame implements ApplicationListener {
	private OrthographicCamera _camera;
	private World _world;
	private Bus _bus;
	private SpriteBatch _batch;
	private Texture _background;
	
	@Override
	public void create() {	
		GroupManager groupManager = new GroupManager();
		TagManager tagManager = new TagManager();
		
		_bus = new Bus(ThreadEnforcer.ANY);
		
		_camera = new OrthographicCamera();
		_camera.setToOrtho(false, 768, 1184);

		_world = new World();	
		_world.setManager(groupManager);
		_world.setManager(tagManager);

		Entity stateEntity = _world.createEntity();
		stateEntity.addComponent(new GameStateComponent());
		stateEntity.addToWorld();
		tagManager.register(Tags.gameState, stateEntity);
		
		BubbleSelectionSystem bubbleSelectionSystem = new BubbleSelectionSystem(_camera, _bus);
		Gdx.input.setInputProcessor(bubbleSelectionSystem);
		_world.setSystem(bubbleSelectionSystem);

		Entity bubbleEntity;
		BubbleComponent bubble;
		BubbleComponent[][] bubbles = new BubbleComponent[GameConstants.columnCount][GameConstants.rowCount];
		for(int col = 0; col < bubbles.length; col++) {
			for(int row = 0; row < bubbles[col].length; row++) {
				bubble = new BubbleComponent();
				bubble.column = col;
				bubble.isRoot = (row == 0);
				bubbles[col][row] = bubble;
				
				bubbleEntity = _world.createEntity();
				bubbleEntity.addComponent(new TransformComponent(getRenderingOffsetForBubble(col, row).add(GameConstants.gridLocation), 
						(float)GameConstants.squareSize,
						(float)GameConstants.squareSize));
				bubbleEntity.addComponent(bubble);
				bubbleEntity.addToWorld();
			}
		}
		
		// Setup bubble links
		for(int col = 0; col < bubbles.length; col++) {
			for(int row = 0; row < bubbles[col].length; row++) {
				bubble = bubbles[col][row];
			    bubble.leftBubble = (col == 0) ? null : bubbles[col-1][row];
			    bubble.rightBubble = (col == bubbles.length - 1) ? null : bubbles[col+1][row];
			    bubble.belowBubble = (row == 0) ? null : bubbles[col][row-1];
			    bubble.aboveBubble = (row == bubbles[col].length - 1) ? null : bubbles[col][row+1];
			}
		}
		
		_world.setSystem(new BubbleCollapsingSystem(_bus));
		_world.setSystem(new BubbleRenderingSystem(_camera));
		_world.setSystem(new BubblePopulationSystem(_bus));
		_world.setSystem(new LevelSystem(_bus));
		_world.setSystem(new ScoringSystem(_bus));
		_world.setSystem(new HudRenderingSystem(_camera));
		_world.initialize();
		_world.setDelta(0);
		_world.process();
		
		_bus.post(new NewGameEvent());
		
		_batch = new SpriteBatch();
		_background = new Texture(Gdx.files.internal("sprites\\background.png"));
	}

	@Override
	public void dispose() {

	}

	@Override
	public void render() {
		_camera.update();
		Gdx.gl.glClearColor(0, 0, 0, 1);
		Gdx.gl.glClear(GL10.GL_COLOR_BUFFER_BIT);

		_batch.setProjectionMatrix(_camera.combined);
		_batch.begin();
		_batch.draw(_background, 0, 0);
		_batch.end();
		
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
	
	private Vector2 getRenderingOffsetForBubble(int column, int row) {
		return new Vector2(calculateOffset(column), calculateOffset(row));
	}
	
	private int calculateOffset(int square) {
		return (square * GameConstants.squareSize) + ((GameConstants.squareSize - GameConstants.bubbleSize) / 2);
	}
}
