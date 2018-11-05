extern crate tcod;

use tcod::console::*;
use tcod::colors;

const SCREEN_WIDTH: i32 = 80;
const SCREEN_HEIGHT: i32 = 50;
const LIMIT_FPS: i32 = 20;

fn main() {
    let mut player_x = SCREEN_WIDTH / 2;
    let mut player_y = SCREEN_HEIGHT / 2;

    let mut root = Root::initializer()
        .font("arial10x10.png", FontLayout::Tcod)
        .font_type(FontType::Greyscale)
        .size(SCREEN_WIDTH, SCREEN_HEIGHT)
        .title("Rust libtcod tutorial")
        .renderer(Renderer::GLSL)
        .init();
    let mut con = Offscreen::new(SCREEN_WIDTH, SCREEN_HEIGHT);

    tcod::system::set_fps(LIMIT_FPS);

    while !root.window_closed() {
        con.set_default_foreground(colors::WHITE);
        con.put_char(player_x, player_y, '@', BackgroundFlag::None);

        root.clear();
        blit(&mut con, (0, 0), (SCREEN_WIDTH, SCREEN_HEIGHT), &mut root, (0, 0), 1.0, 1.0);
        root.flush();

        con.put_char(player_x, player_y, ' ', BackgroundFlag::None);
        let exit = handle_keys(&mut root, &mut player_x, &mut player_y);
        if exit {
            break
        }
    }
}

fn handle_keys(root: &mut Root, x: &mut i32, y: &mut i32) -> bool {
    use tcod::input::Key;
    use tcod::input::KeyCode::*;

    let key = root.wait_for_keypress(true);
    match key {
        Key { code: Escape, .. } => return true,
        Key { code: Up, .. } => *y -= 1,
        Key { code: Down, .. } => *y += 1,
        Key { code: Left, .. } => *x -= 1,
        Key { code: Right, .. } => *x += 1,
        _ => {},
    }
    false
}
