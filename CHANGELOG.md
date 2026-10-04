# Changelog

Format based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [1.2.1] - 2026-10-04

### Changed / 变更

- **Auto-fill / 填原料**：点击时第一格自动改为仓储；第一格为空则按第二格产品识别生产建筑（优先塔内已有建筑）。
- **Vein miners / 矿脉采矿**：普通采矿机按 **座数 × 180/分钟**（6 矿脉×30）；大型 **座数 × 900/分钟**（15×60），线性叠座数，不再用 `ceil(座数/10)` batch。
- **Tech unlock gifts / 科技送塔**：**每项科技**首次解锁送 **1** 座行星内物流塔（2103）；电磁学/基础制造/电磁矩阵/自动化冶金/基础物流仍为 **3** 座。

### Fixed / 修复

- **Vein mining tower / 矿脉采矿塔**：矿脉采矿在 `GetPeriod` 之前执行，避免 `minerTicksPerItem` 未设导致 `period==0` 整站停产。
- **Matrix research tower / 矩阵研究塔**：扣矩阵时写入 `FactoryProductionStat.total[13]`，避免「科技解锁修改」反作弊（`ABN_ItemForUnlockTech`）。

## [1.2.0] - 2026-10-04

### Added / 新增

- **Dyson towers / 戴森塔厂**: Ejector in slot 1 → direct **cell points** (solar sails, stat 11903), no swarm; silo → direct **structure points** (rockets, stat 11902). Cell construction requires node `sp == spMax` (vanilla rule).
- **Dyson swarm cap / 戴森云上限**: Hard cap **10 000** swarm sails via `AddSolarSail`.
- **Matrix research tower / 矩阵研究塔**: Lab in slot 1, matrices from slot 2 onward; feeds UI research queue. Non–universe-matrix tech: instant level when matrices suffice. **Universe-matrix-only** tech: **1 matrix (6006) per lab per second**.
- Re-validate all stations as tower factories on **game load** (`GameMain.Begin`).

### Fixed / 修复

- Ejector cell points: all nodes per wave; sail count cannot go negative; consume capped to available sails.
- **Crash**: `AddTechHash` queued to main thread (`GameMain.Update`) — fixes unlock UI crash from parallel transport tick.
- `build_thunderstore.ps1` runs `package_thunderstore.ps1` after Release build.

## [1.1.2] - 2026-10-04

### Added

- **Station gifts on tech unlock** (2 planetary logistics stations each, item 2103, inventory only; logistics tech/recipes are **not** unlocked): Electromagnetics **1001**, Basic assembling **1201**, Electromagnetic matrix **1002**, Automatic metallurgy **1401**, Basic logistics system **1601**. One grant per tech per save (`featureKey` = `99001000 + techId`).

### 新增

- **科技解锁送塔**（每项 **2** 个行星内物流运输塔，仅进背包，不解锁物流科技/配方）：电磁学 **1001**、基础制造 **1201**、电磁矩阵 **1002**、自动化冶金 **1401**、基础物流系统 **1601**；每科技每存档一次（`featureKey` = `99001000 + techId`）。

## [1.1.1] - 2026-10-03

### Fixed / 修复

- **Self-assembler towers / 自产制造台**: With an empty slot 2, output goes to slot 1 (not the empty product slot).
- **Self-assembler towers / 自产制造台**: Production works with **0** assemblers in slot 1 at the minimum **100-building** rate.
- **Auto-fill / 自动填原料**: Empty slot 2 on assemblers; UI refresh after fill; `SetStationStorage` uses the main player.

### Build / 构建

- Release builds automatically produce `bin/Release/TowerFactory.zip`.

## [1.1.0] - 2026-09-30

### Added / 新增

- Thunderstore dependency **BigTower 1.1.0**.
- Bilingual README, author credit, composite icon.
- Fill-ingredients button and tips: English by default, Chinese on zh-CN.

### Changed / 变更

- Chinese README tagline adds「轮椅」.

## [1.0.0] - 2026-09-30

### Added / 新增

- **Tower Factory** core: storage building in slot 1, product in slot 2, ingredients after; 1 s activation; 1/10 cycle time; `ceil(N/10)` crafts per cycle; no power; ignores proliferator.
- Production, pseudo-recipes, mining, pumps, oil, photons, fractionator, energy exchanger (charge only).
- **Fill ingredients** button in the station window.
- Requires **BepInEx 5.4.17** (xiaoye97).

[1.2.1]: https://github.com/cat-yummi/DSP-towerFactory/compare/v1.2.0...v1.2.1
[1.2.0]: https://github.com/cat-yummi/DSP-towerFactory/compare/v1.1.2...v1.2.0
[1.1.2]: https://github.com/cat-yummi/DSP-towerFactory/compare/v1.1.1...v1.1.2
[1.1.1]: https://github.com/cat-yummi/DSP-towerFactory/compare/v1.1.0...v1.1.1
[1.1.0]: https://github.com/cat-yummi/DSP-towerFactory/compare/v1.0.0...v1.1.0
[1.0.0]: https://github.com/cat-yummi/DSP-towerFactory/releases/tag/v1.0.0
