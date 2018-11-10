extern crate env_logger;
extern crate ggez; 
extern crate log;

use ggez::*;
use log::*;

fn main() {
    env_logger::init();

    trace!("Creating ggez conf and context");
    let c = conf::Conf::new();
    let ctx = &mut Context::load_from_conf("rust_rl", "Jason Olson", c).unwrap();

    trace!("Starting game");
    let state = &mut State::new(ctx).unwrap();
    if let Err(e) = event::run(ctx, state) {
        println!("Error encountered: {}", e);
    } else {
        println!("Game exited cleanly");
    }
}

struct State {
    text: graphics::Text,
    frames: usize
}

impl State {
    fn new(ctx: &mut Context) -> GameResult<State> {
        let font = graphics::Font::new(ctx, "/fonts/Inconsolata-Regular.ttf", 48)?;
        let text = graphics::Text::new(ctx, "Hello World!", &font)?;

        let s = State { text, frames: 0 };
        Ok(s)
    }
}

impl event::EventHandler for State {
    fn update(&mut self, _ctx: &mut Context) -> GameResult<()> {
        Ok(())
    }

    fn draw(&mut self, ctx: &mut Context) -> GameResult<()> {
        graphics::clear(ctx);

        // Drawables are drawn from their top-left corner
        let dest_point = graphics::Point2::new(10.0, 10.0);
        graphics::draw(ctx, &self.text, dest_point, 0.0)?;
        graphics::present(ctx);

        self.frames += 1;
        if (self.frames % 100) == 0 {
            info!("FPS: {}", timer::get_fps(ctx));
        }

        Ok(())
    }
}