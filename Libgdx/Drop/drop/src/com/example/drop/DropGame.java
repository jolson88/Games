package com.example.drop;

import java.util.Iterator;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Input.Keys;
import com.badlogic.gdx.audio.Music;
import com.badlogic.gdx.audio.Sound;
import com.badlogic.gdx.graphics.GL10;
import com.badlogic.gdx.graphics.OrthographicCamera;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.Texture.TextureFilter;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.g2d.TextureRegion;
import com.badlogic.gdx.math.MathUtils;
import com.badlogic.gdx.math.Rectangle;
import com.badlogic.gdx.math.Vector3;
import com.badlogic.gdx.utils.Array;
import com.badlogic.gdx.utils.TimeUtils;

public class DropGame implements ApplicationListener {
	OrthographicCamera camera;
	SpriteBatch batch;
	Texture dropImage;
	Rectangle bucket;
	Texture bucketImage;
	Sound dropSound;
	Music rainMusic;
	Array<Rectangle> raindrops;
	long lastDropTime;
	
	@Override
	public void create() {		
		dropImage = new Texture(Gdx.files.internal("droplet.png"));
		bucketImage = new Texture(Gdx.files.internal("bucket.png"));
		dropSound = Gdx.audio.newSound(Gdx.files.internal("drop.wav"));
		rainMusic = Gdx.audio.newMusic(Gdx.files.internal("rain.mp3"));
		
		bucket = new Rectangle();
		bucket.x = 800 / 2 - bucketImage.getWidth() / 2;
		bucket.y = 20;
		bucket.width = bucketImage.getWidth();
		bucket.height = bucketImage.getHeight();
		
		rainMusic.setLooping(true);
		rainMusic.play();
		
		camera = new OrthographicCamera();
		camera.setToOrtho(false, 800, 480);

		batch = new SpriteBatch();
		
		raindrops = new Array<Rectangle>();
		spawnRaindrop();
	}

	public void spawnRaindrop() {
		Rectangle raindrop = new Rectangle();
		raindrop.x = MathUtils.random(0, 800 - dropImage.getWidth());
		raindrop.y = 480;
		raindrop.width = dropImage.getWidth();
		raindrop.height = dropImage.getHeight();
		
		raindrops.add(raindrop);
		lastDropTime = TimeUtils.nanoTime();
	}
	
	@Override
	public void dispose() {

	}

	@Override
	public void render() {	
		if (Gdx.input.isTouched()) {
			Vector3 touchPos = new Vector3();
			touchPos.set(Gdx.input.getX(), Gdx.input.getY(), 0);
			camera.unproject(touchPos);
			bucket.x = touchPos.x - bucket.getWidth() / 2;
		}
		
		if (Gdx.input.isKeyPressed(Keys.LEFT)) bucket.x -= 200 * Gdx.graphics.getDeltaTime();
		if (Gdx.input.isKeyPressed(Keys.RIGHT)) bucket.x += 200 * Gdx.graphics.getDeltaTime();
		
		if (bucket.x < 0) bucket.x = 0;	
		if (bucket.x > 800 - bucket.getWidth()) bucket.x = 800 - bucket.getWidth();
		
		if (TimeUtils.nanoTime() - lastDropTime > 1000000000) spawnRaindrop();
		
		Iterator<Rectangle> iter = raindrops.iterator();
		while (iter.hasNext()) {
			Rectangle raindrop = iter.next();
			raindrop.y -= 200 * Gdx.graphics.getDeltaTime();
			if (raindrop.y + dropImage.getHeight() < 0) iter.remove();
			if (raindrop.overlaps(bucket)) {
				dropSound.play();
				iter.remove();
			}
		}
		
		camera.update();
		Gdx.gl.glClearColor(0f, 0f, 0.2f, 1f);
		Gdx.gl.glClear(GL10.GL_COLOR_BUFFER_BIT);
		
		batch.setProjectionMatrix(camera.combined);
		batch.begin();
		batch.draw(bucketImage, bucket.x, bucket.y);
		for (Rectangle raindrop: raindrops) {
			batch.draw(dropImage, raindrop.x, raindrop.y);
		}
		batch.end();
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
