# Changelog

Format based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [1.4.2] - 2026-10-31

### Changed

- Production speed is now exactly **N×** for N buildings: each cycle still crafts `ceil(N/10)` times, but the cycle is `base × ceil(N/10) / N` (shortest 1/10 of base). Previously 1–10 buildings all ran at 10×, 11–20 at 20×, etc. Ejector cell points and matrix research keep their fixed timing.

### 变更

- 生产速度严格等于**建筑数倍**：每周期仍生产 `ceil(N/10)` 次，周期改为 `基础周期 × ceil(N/10) / N`（最短为基础周期的 1/10）。此前 1–10 座都是 10 倍速、11–20 座都是 20 倍速。弹射器细胞点和矩阵研究保持原节奏。

## [1.4.1] - 2026-10-19

### Changed

- Thunderstore: **cat_yummi-CatAPI-0.2.4**, **cat_yummi-StationAssistant-0.1.9** (YummiHarness rename).

## [1.4.0] - 2026-10-17

### Changed

- **No longer depends on BigTower**; depends on **Station Assistant** instead.
- New config `[Slots] SlotPreset`: `BigTower` (default) applies the same Station Assistant preset as BigTower (interstellar 9 / planetary 8, expand on build/open); `Off` changes nothing. Skipped automatically when BigTower or SafeBigTower is installed.
- "Fill Ingredients" button moved to the right end of the settings tab row (below the divider), freeing the top-right corner.

### 变更

- **不再依赖 BigTower**，改为依赖 **Station Assistant**。
- 新增配置 `[Slots] SlotPreset`：`BigTower`（默认）自动套用与 BigTower 相同的 Station Assistant 设置（星际 9 / 行星 8 格，建造/打开时扩容）；`Off` 不改任何设置。已安装 BigTower 或 SafeBigTower 时自动跳过。
- 「塔厂：填原料」按钮移到分割线右下（设置标签行最右侧），不再占用右上角。

## [1.3.0] - 2026-10-15

### Changed

- **CatAPI**: all Harmony patches go through `Harness.PatchAll` with per-method `[Guard]` fingerprints; requires `cat_yummi-CatAPI`.

### 变更

- **CatAPI**：全部补丁改为 `Harness.PatchAll` + `[Guard]` 安全注入；需安装 `cat_yummi-CatAPI` 前置。

## [1.2.4] - 2026-10-14

### Fixed

- **Proliferator points**: consuming ingredients or matrices no longer clamps `inc` to `count` (which destroyed Mk.III levels); uses vanilla `split_inc` proportional deduction instead.

### 修复

- **增产点**：消耗原料或研究矩阵时不再错误地把 `inc` 截断为 `count`（会导致 4 增变成 1 增）；改为与原版一致的 `split_inc` 等比扣除。

## [1.2.3] - 2026-10-05

### Added

- **Pseudo recipe**: Miniature Particle Collider — 12 Critical Photons + Annihilation Constraint Sphere + Titanium Alloy → 2 Antimatter Fuel Rods (Mass-Energy Storage ×6 + assembler recipe; photons replace antimatter and hydrogen in the tower).
- **Pseudo recipe**: Oil Refinery — 1 Hydrogen + 1 Coal → 1 Refined Oil (reformed refinement; no oil in ingredient slots; production stats record **2** oil consumed and **3** oil produced per craft, matching the real recipe).

### Fixed

- **Matrix research tower**: Run full research craft on the main thread so `AddTechHash` updates the queue between loop iterations — fixes only completing one tech level or one queued tech per cycle when matrices were sufficient for more.

### 新增

- **伪配方**：微型粒子对撞机 — 12 临界光子 + 湮灭约束球 + 钛合金 → 2 反物质燃料棒（质能储存 ×6 + 组装台配方；塔内用光子替代反物质与氢）。
- **伪配方**：原油精炼厂 — 1 氢 + 1 煤矿 → 1 精炼油（重整精炼；原料格不放油；统计按真实配方记每周期 **消耗 2 油、产出 3 油**）。

### 修复

- **矩阵研究塔**：整段研究逻辑在主线程执行，`AddTechHash` 可在循环内同步推进队列 — 修复矩阵足够时每次周期只完成一级或一项科技的问题。

## [1.2.2] - 2026-10-05

### Fixed

- **Tech unlock gifts**: Skip granting stations while `mainPlayer` is not ready (fixes `NullReferenceException` on new game during `SetForNewGame`).

### Changed

- Thunderstore / r2modman package now includes **CHANGELOG.md**.

### 修复

- **科技送塔**：玩家未生成时不发放物流塔（修复新游戏 `SetForNewGame` 阶段 `TryAddItemToPackage` 空引用崩溃）。

### 变更

- 发布 zip 与 r2modman 部署包含 **CHANGELOG.md**。

## [1.2.1] - 2026-10-04

### Changed

- **Auto-fill**: On click, slot 1 is set to **Storage** if needed; if slot 1 is empty, infer the production building from slot 2’s product (prefer a building already in the station).
- **Vein miners**: **buildings × 180/min** (6 veins × 30) for normal miners; **buildings × 900/min** (15 × 60) for advanced miners; linear with stack count (no `ceil(buildings/10)` batch).
- **Tech unlock gifts**: **1** planetary logistics station (2103) on first unlock of **every** tech; Electromagnetics, Basic assembling, Electromagnetic matrix, Automatic metallurgy, and Basic logistics still grant **3**.

### Fixed

- **Vein mining tower**: Run vein mining before `GetPeriod` so missing `minerTicksPerItem` no longer yields `period == 0` and stops production.
- **Matrix research tower**: Record matrix use in `FactoryProductionStat.total[13]`.

### 变更

- **填原料**：点击时第一格自动改为仓储；第一格为空则按第二格产品识别生产建筑（优先塔内已有建筑）。
- **矿脉采矿**：普通采矿机 **座数 × 180/分钟**（6 矿脉×30）；大型 **座数 × 900/分钟**（15×60），线性叠座数，不再用 `ceil(座数/10)` batch。
- **科技送塔**：**每项科技**首次解锁送 **1** 座行星内物流塔（2103）；电磁学/基础制造/电磁矩阵/自动化冶金/基础物流仍为 **3** 座。

### 修复

- **矿脉采矿塔**：矿脉采矿在 `GetPeriod` 之前执行，避免 `minerTicksPerItem` 未设导致 `period==0` 整站停产。
- **矩阵研究塔**：扣矩阵时写入 `FactoryProductionStat.total[13]`。

## [1.2.0] - 2026-10-04

### Added

- **Dyson towers**: Ejector in slot 1 → direct **cell points** (solar sails, stat 11903), no swarm; silo → direct **structure points** (rockets, stat 11902). Cell construction requires node `sp == spMax` (vanilla rule).
- **Dyson swarm cap**: Hard cap **10 000** swarm sails via `AddSolarSail`.
- **Matrix research tower**: Lab in slot 1, matrices from slot 2 onward; feeds UI research queue. Non–universe-matrix tech: instant level when matrices suffice. **Universe-matrix-only** tech: **1 matrix (6006) per lab per second**.
- Re-validate all stations as tower factories on **game load** (`GameMain.Begin`).

### Fixed

- Ejector cell points: all nodes per wave; sail count cannot go negative; consume capped to available sails.
- **Crash**: `AddTechHash` queued to main thread (`GameMain.Update`) — fixes unlock UI crash from parallel transport tick.
- `build_thunderstore.ps1` runs `package_thunderstore.ps1` after Release build.

### 新增

- **戴森塔厂**：第一格弹射器 → 直接**细胞点**（太阳帆，统计 11903），不入云；发射井 → 直接**结构点**（火箭，统计 11902）。节点须 `sp == spMax`（原版规则）。
- **戴森云上限**：`AddSolarSail` 硬顶 **10000** 帆。
- **矩阵研究塔**：第一格研究站，第二格起放矩阵；供 UI 研究队列。非纯宇宙矩阵：矩阵够则一次完成等级；纯宇宙矩阵：每秒每座研究站 **1** 个 6006。
- 进档 `GameMain.Begin` 后全站重新识别塔厂。

### 修复

- 弹射器细胞点：每波全节点；太阳帆数量不为负；消耗不超过塔内现有帆。
- **崩溃**：`AddTechHash` 改到主线程（`GameMain.Update`），修复并行物流线程弹科技 UI 崩溃。
- `build_thunderstore.ps1` 在 Release 构建后自动执行 `package_thunderstore.ps1`。

## [1.1.2] - 2026-10-04

### Added

- **Station gifts on tech unlock** (2 planetary logistics stations each, item 2103, inventory only; logistics tech/recipes are **not** unlocked): Electromagnetics **1001**, Basic assembling **1201**, Electromagnetic matrix **1002**, Automatic metallurgy **1401**, Basic logistics system **1601**. One grant per tech per save (`featureKey` = `99001000 + techId`).

### 新增

- **科技解锁送塔**（每项 **2** 个行星内物流运输塔，仅进背包，不解锁物流科技/配方）：电磁学 **1001**、基础制造 **1201**、电磁矩阵 **1002**、自动化冶金 **1401**、基础物流系统 **1601**；每科技每存档一次（`featureKey` = `99001000 + techId`）。

## [1.1.1] - 2026-10-03

### Fixed

- **Self-assembler towers**: With an empty slot 2, output goes to slot 1 (not the empty product slot).
- **Self-assembler towers**: Production works with **0** assemblers in slot 1 at the minimum **100-building** rate.
- **Auto-fill**: Empty slot 2 on assemblers; UI refresh after fill; `SetStationStorage` uses the main player.

### Build

- Release builds automatically produce `bin/Release/TowerFactory.zip`.

### 修复

- **自产制造台**：第二格为空时产物写入第一格（不再写入空的产品格）。
- **自产制造台**：第一格 0 座时仍按最低 **100 座**倍率生产。
- **自动填原料**：制造台第二格可为空；填完后刷新 UI；`SetStationStorage` 使用主玩家。

### 构建

- Release 构建自动生成 `bin/Release/TowerFactory.zip`。

## [1.1.0] - 2026-09-30

### Added

- Thunderstore dependency **BigTower 1.1.0**.
- Bilingual README, author credit, composite icon.
- Fill-ingredients button and tips: English by default, Chinese on zh-CN.

### Changed

- Chinese README tagline adds「轮椅」.

### 新增

- Thunderstore 依赖 **BigTower 1.1.0**。
- 双语 README、作者署名、合成图标。
- 填原料按钮与提示：默认英文，zh-CN 为中文。

### 变更

- 中文 README 标语增加「轮椅」。

## [1.0.0] - 2026-09-30

### Added

- **Tower Factory** core: storage building in slot 1, product in slot 2, ingredients after; 1 s activation; 1/10 cycle time; `ceil(N/10)` crafts per cycle; no power; ignores proliferator.
- Production, pseudo-recipes, mining, pumps, oil, photons, fractionator, energy exchanger (charge only).
- **Fill ingredients** button in the station window.
- Requires **BepInEx 5.4.17** (xiaoye97).

### 新增

- **塔厂**核心：第一格仓储建筑、第二格产品、后续原料；稳定 1 秒后生产；周期 1/10；每周期 `ceil(N/10)` 次；不耗电；忽略增产剂。
- 制造、伪配方、采矿、抽水、原油、光子、分馏塔、能量枢纽（仅充电）。
- 运输塔窗口 **填原料** 按钮。
- 需要 **BepInEx 5.4.17**（xiaoye97）。

[1.3.0]: https://github.com/cat-yummi-dspmod/DSP-towerFactory/compare/v1.2.4...v1.3.0
[1.2.4]: https://github.com/cat-yummi-dspmod/DSP-towerFactory/compare/v1.2.3...v1.2.4
[1.2.3]: https://github.com/cat-yummi-dspmod/DSP-towerFactory/compare/v1.2.2...v1.2.3
[1.2.2]: https://github.com/cat-yummi-dspmod/DSP-towerFactory/compare/v1.2.1...v1.2.2
[1.2.1]: https://github.com/cat-yummi-dspmod/DSP-towerFactory/compare/v1.2.0...v1.2.1
[1.2.0]: https://github.com/cat-yummi-dspmod/DSP-towerFactory/compare/v1.1.2...v1.2.0
[1.1.2]: https://github.com/cat-yummi-dspmod/DSP-towerFactory/compare/v1.1.1...v1.1.2
[1.1.1]: https://github.com/cat-yummi-dspmod/DSP-towerFactory/compare/v1.1.0...v1.1.1
[1.1.0]: https://github.com/cat-yummi-dspmod/DSP-towerFactory/compare/v1.0.0...v1.1.0
[1.0.0]: https://github.com/cat-yummi-dspmod/DSP-towerFactory/releases/tag/v1.0.0
