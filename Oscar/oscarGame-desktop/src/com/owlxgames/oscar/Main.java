package com.owlxgames.oscar;

import com.badlogic.gdx.backends.lwjgl.LwjglApplication;
import com.badlogic.gdx.backends.lwjgl.LwjglApplicationConfiguration;

public class Main {
	public static void main(String[] args) {
		LwjglApplicationConfiguration cfg = new LwjglApplicationConfiguration();
		cfg.title = "oscarGame";
		cfg.useGL20 = true;
		cfg.width = 480;
		cfg.height = 740;
		
		new LwjglApplication(new OscarGame(), cfg);
	}
}
