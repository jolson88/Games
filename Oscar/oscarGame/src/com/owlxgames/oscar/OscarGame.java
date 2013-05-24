package com.owlxgames.oscar;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.*;
import com.badlogic.gdx.graphics.g2d.*;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.physics.box2d.*;
import com.badlogic.gdx.physics.box2d.BodyDef.BodyType;

public class OscarGame implements ApplicationListener {
	private static final float PHYSICS_STEP = 1/60f;
	private static final float WORLD_TO_BOX = 0.01f;
	private static final float BOX_TO_WORLD = 100f;
	private static final float GRAVITY = -2.5f;
	
	private float accumulator = 0f;
	private World world;
	private OrthographicCamera camera;
	private Box2DDebugRenderer renderer;
	
	@Override
	public void create() {			
		camera = new OrthographicCamera();
		camera.setToOrtho(false, 800, 480);
		
		renderer = new Box2DDebugRenderer();		
		world = new World(new Vector2(0, GRAVITY), true);		
		createShapes();
	}
	
	private void createShapes() {
		createCircularBody(world, BodyType.DynamicBody, new Vector2(100, 300), 16f, 0.5f, 0.6f);
		createRectangleBody(world, BodyType.StaticBody, new Vector2(10, 10), 780, 10, 0f);
		createRectangleBody(world, BodyType.DynamicBody, new Vector2(300, 200), 32f, 32f, 0f);
		createRectangleBody(world, BodyType.DynamicBody, new Vector2(310, 140), 32f, 32f, 1f);
	}
	
	@Override
	public void dispose() {
		renderer.dispose();
		world.dispose();
	}

	@Override
	public void render() {
		accumulator += Gdx.graphics.getDeltaTime();
		while (accumulator >= PHYSICS_STEP) {
			world.step(PHYSICS_STEP, 6, 6);
			accumulator -= PHYSICS_STEP;
		}
		
		camera.update();
		Gdx.gl.glClearColor(0, 0, 0, 1);
		Gdx.gl.glClear(GL10.GL_COLOR_BUFFER_BIT);

		renderer.render(world, camera.combined.cpy().scl(BOX_TO_WORLD));
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
	
	private void createRectangleBody(World world, BodyType type, Vector2 position, float width, float height, float angle) {
		float scaledWidth = width * WORLD_TO_BOX;
		float scaledHeight = height * WORLD_TO_BOX;
		float scaledX = position.x * WORLD_TO_BOX;
		float scaledY = position.y * WORLD_TO_BOX;
		
		BodyDef bodyDef = new BodyDef();
		bodyDef.type = type;
		
		// Box2D expects positions in the center, while our world coordinates are bottom left
		bodyDef.position.set(scaledX + scaledWidth / 2, scaledY + scaledHeight / 2);
		bodyDef.angle = angle;
		Body body = world.createBody(bodyDef);
		
		PolygonShape groundShape = new PolygonShape();
		// Width and Height in Box2D are half-width and half-height
		groundShape.setAsBox(scaledWidth / 2, scaledHeight / 2);
		FixtureDef fixtureDef = new FixtureDef();
		fixtureDef.shape = groundShape;
		fixtureDef.density = 1f;
		body.createFixture(fixtureDef);
		
		groundShape.dispose();
	}
	
	private void createCircularBody(World world, BodyType type, Vector2 position, float radius, float friction, float restitution) {
		float scaledRadius = radius * WORLD_TO_BOX;
		float scaledX = position.x * WORLD_TO_BOX;
		float scaledY = position.y * WORLD_TO_BOX;
		
		BodyDef bodyDef = new BodyDef();
		bodyDef.type = type;
		bodyDef.position.set(scaledX + scaledRadius, scaledY + scaledRadius);
		Body body = world.createBody(bodyDef);

		CircleShape circle = new CircleShape();
		circle.setRadius(scaledRadius);

		FixtureDef fixtureDef = new FixtureDef();
		fixtureDef.shape = circle;
		fixtureDef.density = 1f;
		fixtureDef.friction = friction;
		fixtureDef.restitution = restitution;
		body.createFixture(fixtureDef);

		circle.dispose();
	}
}
