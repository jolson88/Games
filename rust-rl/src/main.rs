extern crate env_logger;
extern crate ggez;
extern crate log;
extern crate rand;

use ggez::*;
use log::*;
use rand::{thread_rng, Rng, ThreadRng};

const MAP_WIDTH: u32 = 100;
const MAP_HEIGHT: u32 = 33;

fn main() -> GameResult<()> {
    env_logger::init();

    trace!("Creating ggez conf and context");
    let c = conf::Conf::new();
    let ctx = &mut Context::load_from_conf("rust_rl", "Jason Olson", c)?;

    trace!("Starting game");
    let state = &mut State::new(ctx)?;
    event::run(ctx, state)?;

    Ok(())
}

struct State {
    cells: Vec<Vec<GridCell>>,
    rng: ThreadRng,
}

impl State {
    fn new(ctx: &mut Context) -> GameResult<State> {
        graphics::set_background_color(ctx, graphics::Color::new(0.0, 0.0, 0.0, 0.0));

        let w = ctx.conf.window_mode.width;
        let h = ctx.conf.window_mode.height;
        let cell_width = w as f32 / MAP_WIDTH as f32;
        let cell_height = h as f32 / MAP_HEIGHT as f32;
        let mut cells = Vec::new();
        for row in 0..MAP_HEIGHT {
            let mut cols = Vec::new();
            for col in 0..MAP_WIDTH {
                cols.push(GridCell::new(col, row, cell_width, cell_height));
            }
            cells.push(cols);
        }
        let s = State {
            cells: cells,
            rng: thread_rng(),
        };
        Ok(s)
    }
}

impl event::EventHandler for State {
    fn update(&mut self, _ctx: &mut Context) -> GameResult<()> {
        for col in 0..MAP_WIDTH {
            for row in 0..MAP_HEIGHT {
                self.cells[row as usize][col as usize].color =
                    graphics::Color::new(0.0, 0.0, self.rng.gen(), 1.0);
            }
        }
        Ok(())
    }

    fn draw(&mut self, ctx: &mut Context) -> GameResult<()> {
        graphics::clear(ctx);

        for col in 0..MAP_WIDTH {
            for row in 0..MAP_HEIGHT {
                // @slow graphics::rectangle creates a new mesh everytime. Create our own to optimize this later.
                let cell = self.cells[row as usize][col as usize];
                graphics::set_color(ctx, cell.color)?;
                graphics::rectangle(ctx, graphics::DrawMode::Fill, cell.rect)?;
            }
        }

        println!("FPS: {}", timer::get_fps(ctx));
        graphics::present(ctx);
        Ok(())
    }
}

#[derive(Clone, Copy, Debug)]
struct GridCell {
    color: graphics::Color,
    rect: graphics::Rect,
}

impl GridCell {
    fn new(column: u32, row: u32, pixel_width: f32, pixel_height: f32) -> GridCell {
        let r = graphics::Rect::new(
            column as f32 * pixel_width,
            row as f32 * pixel_height,
            pixel_width,
            pixel_height,
        );
        GridCell {
            color: graphics::BLACK,
            rect: r,
        }
    }
}
