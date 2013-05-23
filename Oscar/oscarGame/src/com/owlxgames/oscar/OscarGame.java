package com.owlxgames.oscar;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.*;
import com.badlogic.gdx.graphics.g2d.*;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.physics.box2d.*;
import com.badlogic.gdx.physics.box2d.BodyDef.BodyType;

public class OscarGame implements ApplicationListener {
	private World world;
	private OrthographicCamera camera;
	private Box2DDebugRenderer renderer;
	
	@Override
	public void create() {			
		camera = new OrthographicCamera();
		camera.setToOrtho(false, 800, 480);
		
		renderer = new Box2DDebugRenderer();
		
		world = new World(new Vector2(0, -20), true);
		
		// First we create a body definition
		BodyDef bodyDef = new BodyDef();
		// We set our body to dynamic, for something like ground which doesnt move we would set it to StaticBody
		bodyDef.type = BodyType.DynamicBody;
		// Set our body's starting position in the world
		bodyDef.position.set(100, 300);

		// Create our body in the world using our body definition
		Body body = world.createBody(bodyDef);

		// Create a circle shape and set its radius to 6
		CircleShape circle = new CircleShape();
		circle.setRadius(16f);

		// Create a fixture definition to apply our shape to
		FixtureDef fixtureDef = new FixtureDef();
		fixtureDef.shape = circle;
		fixtureDef.density = 0.5f; 
		fixtureDef.friction = 0.4f;
		fixtureDef.restitution = 0.6f; // Make it bounce a little bit

		// Create our fixture and attach it to the body
		Fixture fixture = body.createFixture(fixtureDef);

		// Remember to dispose of any shapes after you're done with them!
		// BodyDef and FixtureDef don't need disposing, but shapes do.
		circle.dispose();
	}

	@Override
	public void dispose() {
		renderer.dispose();
		world.dispose();
	}

	@Override
	public void render() {
		world.step(1/45f, 6, 2);
		
		camera.update();
		Gdx.gl.glClearColor(0, 0, 1, 1);
		Gdx.gl.glClear(GL10.GL_COLOR_BUFFER_BIT);

		renderer.render(world, camera.combined);
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
