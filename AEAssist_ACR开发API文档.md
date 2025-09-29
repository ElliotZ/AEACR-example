# AEAssist 本体 ACR 开发 API 文档

本文档整理了 AEAssist 本体（排除 ACR 目录）提供的所有可在 ACR（Action Combat Routine）开发时调用的方法和类。

## 目录

- [核心类](#核心类)
- [内存API (MemoryApi)](#内存api-memoryapi)
- [职业API (JobApi)](#职业api-jobapi)
- [辅助类 (Helper)](#辅助类-helper)
- [扩展方法 (Extension)](#扩展方法-extension)
- [战斗相关类型定义](#战斗相关类型定义)
- [使用示例](#使用示例)

## 核心类

### Core 类
位置：`AEAssist/Core/Core.cs`

```csharp
// 获取当前玩家对象
public static IPlayerCharacter Me => Svc.ClientState.LocalPlayer

// 获取指定类型的服务实例（依赖注入）
public static T Resolve<T>() where T : class

// 示例
var spell = Core.Resolve<MemApiSpell>();
var buff = Core.Resolve<MemApiBuff>();
```

## 内存API (MemoryApi)

所有 MemoryApi 类都使用 `[Autofac()]` 标记，通过 `Core.Resolve<T>()` 获取实例。

### MemApiSpell - 技能施法管理
```csharp
// 基础功能
void CancelCast() // 打断读条
void PetMove(Vector3 target, uint actionId = 3) // 宠物移动
uint GetId(string name) // 根据技能名称获取ID
SpellTargetType GetSkillTarget(uint id) // 获取技能目标类型

// 技能施放
bool Cast(uint id, IBattleChara battleChara, bool dontUseGcdOpt = false) // 对目标施放技能
bool Cast(uint id, Vector3 pos) // 在指定位置施放技能
bool QueueSpell(uint id, IBattleChara battleChara) // 排队技能

// 技能状态检查
uint GetActionState(uint id) // 获取技能状态码
uint CheckActionChange(uint id) // 检查技能变化（如连击）
bool CheckActionCanUse(uint actionId, float gcdTimel = 0.5f, float abilityTimel = 0.1f) // 检查技能是否可用
float GetActionRange(uint id) // 获取技能范围

// 冷却和充能
TimeSpan GetRecastTime(uint id) // 获取技能总冷却时间
TimeSpan GetCooldown(uint id) // 获取技能剩余冷却时间
float GetCharges(uint id) // 获取技能充能层数
float GetChargesMaxCount(uint actionId) // 获取最大充能数

// GCD相关
int GetGCDDuration(bool raw) // 获取GCD持续时间
int GetElapsedGCD() // 获取已过GCD时间
bool IsAnimationLock // 是否在动画锁定中

// 连击和队列
uint GetLastComboSpellId() // 获取上一个连击技能
bool HasActionQueue // 是否有技能队列
uint QueuedActionId // 队列中的技能ID
float AnimationLock // 动画锁定时间

// 范围和视线检查
uint GetActionInRangeOrLoS(uint actionId) // 检查技能范围和视线(对当前目标)
bool CheckActionInRangeOrLoS(uint actionId, IBattleChara target) // 检查技能范围和视线(对指定目标)
```

### MemApiBuff - Buff管理
```csharp
// Buff检查
bool HasAura(IBattleChara battleChara, uint auraId, int timeLeft = 0) // 检查是否有指定Buff
bool HasMyAura(IBattleChara battleChara, uint auraId) // 检查是否有自己释放的Buff
bool HasAnyAura(IBattleChara battleChara, List<uint> auras, int msLeft = 0) // 检查是否有列表中的任意Buff
uint HitAnyAura(IBattleChara battleChara, List<uint> auras, int msLeft = 0) // 返回命中的第一个Buff ID

// Buff信息
int GetStack(IBattleChara battleChara, uint id) // 获取Buff层数
int GetAuraTimeleft(IBattleChara battleChara, uint id, bool fromMe) // 获取Buff剩余时间(毫秒)
uint BuffParam(IBattleChara target, uint buffId) // 获取Buff参数
ushort BuffStackCount(IBattleChara battleChara, uint auraId) // 获取Buff层数

// 驱散检查
bool HasCanDispel(IBattleChara battleChara) // 是否有可驱散的Buff
```

### MemApiTarget - 目标管理
```csharp
// 目标操作
void SetTarget(IGameObject? tar) // 设置目标
void ClearTarget() // 清除目标
IGameObject CurrTarget(IBattleChara unit) // 获取单位的当前目标

// 目标检查
bool CanAttack(IGameObject target, uint actionId = 142) // 是否可以攻击目标
bool IsBoss(IBattleChara actor) // 是否为Boss
bool HasPositional(IGameObject gameObject) // 是否有身位

// 目标状态
float CurrentHealthPercent(IBattleChara battleChara) // 当前血量百分比
float CurrentManaPercent(IBattleChara battleChara) // 当前魔法值百分比

// 目标查找
bool TryFindUnit(string name, float distance, out IGameObject agent) // 按名称查找单位
bool TryFindUnit(uint npcId, bool targetable, out IGameObject agent) // 按ID查找单位

// 身位检查
bool IsFlanking // 是否在侧翼
bool IsBehind // 是否在背后
```

### MemApiMove - 移动控制
```csharp
// 移动状态
bool IsMoving() // 是否在移动中

// 移动控制
void MoveToTarget(Vector3 targetPoint) // 移动到指定位置
void CancelMove() // 取消移动
void AutoMove(bool value) // 自动移动开关

// 朝向控制
void SetRot(float rotation, bool withSendKey = true) // 设置角色朝向
void Face2Target(IBattleChara target, bool isBack = false) // 面向目标

// 特殊移动
void SlideTp(Vector3 target, long time) // 滑步传送
void LockPos(Vector3 target, long duration) // 锁定位置

// 路径
bool PathEnabled // 路径是否启用
```

### MemApiCondition - 状态检测
```csharp
bool IsBoundByDuty() // 是否在副本中
bool IsInCombat() // 是否在战斗中
bool IsInCutscene() // 是否在过场动画中
bool IsInQuestEvent() // 是否在任务事件中
bool IsBetweenAreas() // 是否在区域间传送中
bool IsInIslandSanctuary() // 是否在无人岛
bool IsCrafting() // 是否在制作中
bool IsGathering() // 是否在采集中
bool IsInBardPerformance() // 是否在演奏中
```

### MemApiDuty - 副本管理
```csharp
// 副本状态
bool IsBoundByDuty() // 是否在副本中
string DutyName() // 副本名称
int DutyMembersNumber() // 副本人数
DutySchedule? GetSchedule() // 获取副本进度信息
bool IsInBossRoom() // 是否在Boss房间

// 副本属性
ContentFinderCondition? DutyInfo // 副本信息
bool InMission // 是否正式开始
bool InBossBattle // 是否在Boss战斗中
bool IsOver // 副本是否结束
```

### MemApiAddon - UI窗口交互
```csharp
// 窗口检查
bool CheckAddon(string addonName) // 检测窗口是否存在
bool IsAddonAndNodesReady(string addonName) // 检查窗口和节点是否就绪
Task<bool> WaitAddonUntil(string addonName, bool visible = true, int timeout = 5000, int delay = 300) // 等待窗口状态

// 窗口操作
void SetAddonValue(string addonName, uint nodeIndex, Int32 values) // 设置控件数值
void SetAddonClicked(string addonName, params object[] values) // 点击窗口按钮
string GetNodeText(string addonName, params int[] nodeNumbers) // 获取节点文本
AddonValue GetAddonValue(string addonName, uint index) // 获取窗口数据

// 交互
bool InteractWithUnit(uint objectId, bool checklineOnSight = true) // 与游戏内单位交互
string GetQuestNameFromTodoList(int index = 1) // 从任务列表获取任务名称
```

### MemApiChatMessage - 聊天和提示
```csharp
// 聊天发送
void Say(string message) // 说话频道
void Shout(string message) // 呼喊频道
void Party(string message) // 小队频道

// 提示消息
void PrintPluginMessage(string msg) // 打印插件消息
void Toast(string msg) // 显示Toast提示
void Toast2(string msg, int s, int time) // 显示带样式的文本提示

// 其他
uint LastLinkedItemId() // 最后链接的物品ID
```

### MemApiParty - 小队管理
```csharp
List<IBattleChara> GetParty() // 获取小队成员列表
IEnumerable<IBattleChara> GetMembers() // 获取小队成员
IBattleChara? Mo() // 获取鼠标悬停目标

// 属性
Dictionary<int, IBattleChara> PartyList // 小队成员字典
```

### MemApiZoneInfo - 区域信息
```csharp
uint GetCurrTerrId() // 获取当前地图ID
```

### MemApiSpellCastSuccess - 技能成功施放记录
```csharp
uint LastGcd // 上一个GCD技能
uint LastAbility // 上一个能力技能
```

### MemApiCountdown - 倒计时
```csharp
bool IsActive() // 倒计时是否激活
float TimeRemaining() // 剩余时间
float Timer() // 倒计时器值
```

### MemApiHotkey - 热键检测
```csharp
bool CheckState(ModifierKey modifierKey, Keys key) // 检查按键状态
bool CheckStateDown(ModifierKey modifierKey, Keys key) // 检查按键按下状态
Keys GetAnyKeyInput() // 获取任意按键输入

// 事件
KeyBoardEvent OnKeyBoardDownOrUp // 键盘按下或释放事件
```

### MemApiIcon - 图标资源
```csharp
bool GetActionTexture(uint id, out IDalamudTextureWrap? textureWrap, bool isAdjust = true) // 获取技能图标
bool GetIconTexture(int iconId, out IDalamudTextureWrap textureWrap, bool hq = true) // 获取物品图标
uint GetJobIconId(string jobName) // 获取职业图标ID
uint GetJobIconIdByJob(Jobs job) // 根据职业枚举获取图标ID
```

### MemApiMacro - 宏命令
```csharp
bool UseMacro(MacroItem macroItem) // 执行宏命令
```

## 职业API (JobApi)

所有职业API都继承自对应的 Dalamud JobGauge 类型，通过 `Core.Resolve<JobApi_职业名>()` 获取。

### JobApi_WhiteMage - 白魔法师
```csharp
int Lily // 百合花数量
int BloodLily // 血百合数量
short LilyTimer // 百合计时器
```

### JobApi_Monk - 武僧
```csharp
byte Chakra // 斗气数量
BeastChakra[] BeastChakra // 震脚状态数组
Nadi Nadi // 元气状态
int OpoOpoFury // 连击1能量
int RaptorFury // 连击2能量
int CoeurlFury // 连击3能量
ushort BlitzTimeRemaining // 极意时间剩余
```

### JobApi_Sage - 贤者
```csharp
long AddersgallTimer // 蛇刺计时器
int Addersgall // 蛇刺数量
int Addersting // 蛇毒数量
bool Eukrasia // 均衡状态
```

### 其他职业API
每个职业都有对应的 JobApi 类，提供该职业特有的量谱信息：
- JobApi_Astrologian - 占星术士
- JobApi_Bard - 吟游诗人
- JobApi_BlackMage - 黑魔法师
- JobApi_Dancer - 舞者
- JobApi_DarkKnight - 暗黑骑士
- JobApi_Dragoon - 龙骑士
- JobApi_GunBreaker - 绝枪战士
- JobApi_Machinist - 机工士
- JobApi_Ninja - 忍者
- JobApi_Paladin - 骑士
- JobApi_Pictomancer - 绘灵法师
- JobApi_Reaper - 钐镰师
- JobApi_RedMage - 赤魔法师
- JobApi_Samurai - 武士
- JobApi_Scholar - 学者
- JobApi_Summoner - 召唤师
- JobApi_Viper - 蝰蛇
- JobApi_Warrior - 战士

## 辅助类 (Helper)

### PartyHelper - 小队管理辅助
位置：`AEAssist/API/Helper/PartyManager.cs`

```csharp
// 静态列表（自动更新）
static List<IBattleChara> DeadAllies // 死亡单位列表
static List<IBattleChara> Party // 整个队伍成员
static List<IBattleChara> CastableParty // 可施法的队友(包括自己)
static List<IBattleChara> CastableTanks // 坦克列表
static List<IBattleChara> CastableHealers // 治疗列表
static List<IBattleChara> CastableDps // DPS列表
static List<IBattleChara> CastableMainTanks // 主坦克列表
static List<IBattleChara> CastableAlliesWithin30 // 30米内可施法队友
static List<IBattleChara> CastableAlliesWithin25 // 25米内
static List<IBattleChara> CastableAlliesWithin20 // 20米内
static List<IBattleChara> CastableAlliesWithin15 // 15米内
static List<IBattleChara> CastableAlliesWithin10 // 10米内
static List<IBattleChara> CastableAlliesWithin3 // 3米内
static List<IBattleChara> CastableMelees // 近战职业
static List<IBattleChara> CastableRangeds // 远程职业
```

### TargetHelper - 目标辅助
位置：`AEAssist/API/Helper/TargetHelper.cs`

```csharp
// 敌人数量统计
int GetNearbyEnemyCount(IBattleChara target, int spellCastRange, int damageRange) // 获取目标附近敌人数量
int GetNearbyEnemyCount(int range) // 获取自身周围敌人数量
int GetEnemyCountInsideSector(IBattleChara me, IBattleChara target, float sectorRadius, float sectorAngle) // 扇形范围内敌人数量

// 目标检查
bool IsBoss(IBattleChara target) // 是否为Boss
float GetTargetDistanceFromMeTest2D(IBattleChara target, IBattleChara origin) // 2D距离计算
bool targetCastingIsBossAOE(IBattleChara target, int timeLeft) // 目标是否准备放AOE
```

### GCDHelper - GCD管理辅助
位置：`AEAssist/API/Helper/GCDHelper.cs`

```csharp
bool Is2ndAbilityTime(float timeInMs = 1000f) // 是否为插入能力技时机
int GetGCDDuration() // 获取GCD总时间
int GetGCDCooldown() // 获取GCD剩余冷却
int GetElapsedGCD() // 获取已过GCD时间
bool CanUseGCD() // 是否可以使用GCD技能（考虑队列时间）
bool CanUseOffGcd() // 是否可以使用能力技
```

### SpellHelper - 技能辅助
位置：`AEAssist/API/Helper/SpellHelper.cs`

```csharp
bool CanUseAction() // 检查当前是否可以使用技能（综合状态检查）
IBattleChara? GetTarget(SpellTargetType spellTargetType) // 根据目标类型获取目标
IBattleChara? GetTarget(Spell spell) // 获取技能的目标
void PrintSpell() // 打印所有技能信息（调试用）
```

### DotBlacklistHelper - DOT黑名单辅助
位置：`AEAssist/API/Helper/DotBlacklistHelper.cs`

```csharp
bool IsBlackList(IBattleChara gameObject) // 检查目标是否在DOT黑名单中
```

### LogHelper - 日志辅助
位置：`AEAssist/API/Helper/LogHelper.cs`

```csharp
void Debug(string message) // 调试日志
void Info(string message) // 信息日志
void Print(string category, string message) // 分类日志
void Error(string message) // 错误日志
```

### 其他常用Helper类
- **ChatHelper** - 聊天相关功能
- **ECHelper** - ECommons相关功能
- **ItemHelper** - 物品相关功能
- **JobHelper** - 职业相关功能
- **MathHelper** - 数学计算辅助
- **MoveHelper** - 移动相关辅助
- **StatusHelper** - 状态相关辅助
- **TimeHelper** - 时间相关辅助
- **WeatherHelper** - 天气相关辅助

## 扩展方法 (Extension)

### IGameObject 扩展
位置：`AEAssist/API/Extension/GameObjectExtension/GameObjectExtension.GameObject.cs`

```csharp
// 身份检查
bool IsMe() // 是否是自己
bool IsLocalPlayer() // 是否是本地玩家（同IsMe）
Relationship GetRelationshipWithLocalPlayer() // 获取与玩家的关系(Self/Party/Friendly/Hostile)

// 位置和距离
float Distance(IGameObject target, DistanceMode mode = DistanceMode.IgnoreAll) // 计算两目标距离
bool InActionRange(float range = 30) // 是否在技能范围内
Vector3 GetEyePostion() // 获取眼睛位置（头顶上方2米）
float DistanceToPlayer() // 到玩家的距离

// 身位检查
bool HasPositional() // 是否有身位
bool InBehind(Vector3 pos, bool checkPositional = true) // 点是否在背后
bool IsBehindTarget(IGameObject target) // 是否在目标背后
SectorShape BehindShape() // 获取背后扇形区域

// 目标和交互
void BecomeTargetOfLocalPlayer() // 成为玩家目标
bool TargetInteract() // 与目标交互

// 战斗相关
bool IsEnemy() // 是否是敌人
bool IsInEnemiesList() // 是否在仇恨列表内
bool EventValid() // 事件是否有效

// 类型转换
GameObject* ToStruct() // 转换为结构体指针
T? ToGameObject<T>(this uint objectId) where T : class, IGameObject // 从ID获取对象

// 其他
bool IsPvP() // 是否在PvP中
uint GetNamePlateIcon() // 获取名牌图标
EventHandlerContent GetEventType() // 获取事件类型
uint FateId() // 获取FATE ID
uint EventId() // 获取事件ID
string ToLogString() // 转换为日志字符串
```

### IBattleChara 扩展
位置：`AEAssist/API/Extension/GameObjectExtension/GameObjectExtension.BattleCharacter.cs`

```csharp
// Buff管理
bool HasAura(uint auraId, int timeLeft = 0) // 是否有指定Buff
bool HasLocalPlayerAura(uint auraId) // 是否有玩家施加的Buff
int GetAuraStack(uint id) // 获取Buff层数
bool HasCanDispel() // 是否有可驱散Buff
bool HasMyAuraWithTimeleft(uint id, int timeLeft = 0) // 自己的Buff剩余时间是否大于指定值
bool ContainsMyInEndAura(uint id, int timeLeft = 0) // 检查即将结束的Buff
bool HasAnyAura(List<uint> auras, int msLeft = 0) // 是否有列表中的任意Buff
uint HitAnyAura(List<uint> auras, int msLeft = 0) // 返回命中的第一个Buff ID

// 目标管理
IBattleChara? GetCurrTarget() // 获取当前目标
IBattleChara? GetCurrTargetsTarget() // 获取目标的目标
bool TryGetCurrTarget(out IBattleChara? battleChara) // 尝试获取当前目标

// 战斗检查
bool ValidAttackUnit() // 是否为有效攻击单位
bool CanAttackUnit() // 是否可以攻击
bool CanAttack() // 是否可以攻击（简化版）
bool ValidUnit() // 是否为有效单位
bool NotInvulnerable() // 是否不是无敌状态
bool InRange(IBattleChara target, int range = 3) // 是否在范围内

// 特殊检查
bool IsBoss() // 是否是Boss
bool IsDummy() // 是否是木桩
bool IsPlayerCamp() // 是否是玩家阵营
bool IsDead() // 是否死亡
```

### IPlayerCharacter 扩展
位置：`AEAssist/API/Extension/GameObjectExtension/GameObjectExtension.PlayerCharacter.cs`

```csharp
bool InCutSceneState() // 是否在过场动画状态（仅对自己有效）
```

### ICharacter 扩展
位置：`AEAssist/API/Extension/GameObjectExtension/GameObjectExtension.Character.cs`

```csharp
// 属性获取
float CurrentHpPercent() // 当前血量百分比
float CurrentMpPercent() // 当前魔法值百分比

// 职业检查
bool IsTank() // 是否是坦克
bool IsHealer() // 是否是治疗
bool IsDps() // 是否是DPS
bool IsMelee() // 是否是近战
bool IsRanged() // 是否是远程
bool IsCaster() // 是否是法系

// 状态检查
bool InCombat() // 是否在战斗中
bool IsValid() // 是否有效（血量>0且不是自己）
```

### Spell 扩展
位置：`AEAssist/API/Helper/SpellExtension.cs`

```csharp
bool IsReady() // 技能是否就绪
bool IsReadyWithCanCast() // 技能是否就绪且可以施放
bool RecentlyUsed(int time = 1200) // 技能是否刚使用过
Spell GetSpell(this uint spellId) // 从ID创建Spell对象
```

## 战斗相关类型定义

### Spell 类
位置：`AEAssist/Internal/Plugins/CombatRoutine/Define/Spell.cs`

```csharp
// 技能目标类型枚举
enum SpellTargetType
{
    Target = 0,          // 当前目标
    Self = 1,            // 自己
    TargetTarget = 2,    // 当前目标的目标(tt)
    Pm1-Pm8,             // 小队列表1-8
    SpecifyTarget = 100, // 特殊目标(需要id)
    Location = 101,      // 地点
    DynamicTarget = 102, // 动态目标
    MapCenter = 103      // 场地中心
}

// 技能类别枚举
enum SpellCategory
{
    Default,    // 默认
    LimitBreak, // 极限技
    Potion,     // 爆发药
    Sprint,     // 疾跑
    Dance,      // 跳舞(舞者)
    Item        // 道具
}

// Spell 类构造函数
public Spell(uint id, SpellTargetType targetIndex) // 创建指定目标类型的技能
public Spell(uint id, IBattleChara target) // 创建指定目标的技能
public Spell(uint id, Vector3 location) // 创建地面目标技能

// 属性
public uint Id // 技能ID
public SpellTargetType SpellTargetType // 目标类型
public SpellCategory SpellCategory // 技能类别
public IBattleChara? SpecifyTarget // 指定目标
public Vector3 SpecifyLocation // 指定位置
```

### AI 类
位置：`AEAssist/Internal/Plugins/CombatRoutine/Module/AI.cs`

```csharp
// 获取AI实例
public static AI Instance

// 战斗数据
public BattleData BattleData // 战斗数据管理

// 高优先级技能队列
public Queue<Spell> HighPrioritySlots_GCD // 高优先级GCD队列
public Queue<Spell> HighPrioritySlots_OffGCD // 高优先级能力技队列
```

### SettingMgr - 设置管理器
位置：`AEAssist/Internal/Plugins/CombatRoutine/Setting/SettingMgr.cs`

```csharp
// 获取设置
public static T GetSetting<T>() where T : BaseSetting

// 常用设置类
GeneralSettings // 通用设置
HotkeySetting // 热键设置
RotationSetting // 循环设置
PotionSetting // 爆发药设置
```

### AurasDefine - Buff定义
位置：`AEAssist/Internal/Plugins/CombatRoutine/Define/AurasDefine.cs`

包含游戏中各种Buff的ID定义，例如：
- 无敌类Buff
- 减伤类Buff
- 增益类Buff
- 负面状态等

### Jobs 枚举
位置：`AEAssist/Internal/Plugins/CombatRoutine/Define/Jobs.cs`

定义所有职业的枚举值。

## 使用示例

### 基础使用
```csharp
// 检查自身状态
if (Core.Me.CurrentHpPercent() < 50) {
    // 血量低于50%时的处理
}

// 检查Buff
if (Core.Me.HasAura(123)) {
    // 有指定Buff时的处理
}

// 获取目标
var target = Core.Me.GetCurrTarget();
if (target != null && target.CanAttack()) {
    // 对目标进行操作
}
```

### 使用内存API
```csharp
// 技能操作
var spellApi = Core.Resolve<MemApiSpell>();
if (spellApi.CheckActionCanUse(123)) {
    spellApi.Cast(123, target);
}

// Buff检查
var buffApi = Core.Resolve<MemApiBuff>();
if (buffApi.HasMyAura(Core.Me, 456)) {
    var timeLeft = buffApi.GetAuraTimeleft(Core.Me, 456, true);
    // 根据剩余时间做处理
}

// 移动控制
var moveApi = Core.Resolve<MemApiMove>();
if (!moveApi.IsMoving()) {
    moveApi.Face2Target(target);
}
```

### 使用职业API
```csharp
// 白魔法师
var whm = Core.Resolve<JobApi_WhiteMage>();
if (whm.Lily >= 3) {
    // 使用百合技能
}

// 武僧
var mnk = Core.Resolve<JobApi_Monk>();
if (mnk.Chakra >= 5) {
    // 使用斗气技能
}
```

### 使用辅助类
```csharp
// 队伍管理
var lowestHpMember = PartyHelper.CastableAlliesWithin30
    .Where(m => m.CurrentHp > 0)
    .OrderBy(m => m.CurrentHpPercent())
    .FirstOrDefault();

if (lowestHpMember != null && lowestHpMember.CurrentHpPercent() < 30) {
    // 治疗最低血量成员
}

// 目标计数
var enemyCount = TargetHelper.GetNearbyEnemyCount(10);
if (enemyCount >= 3) {
    // 使用AOE技能
}

// GCD管理
if (GCDHelper.CanUseGCD()) {
    // 使用GCD技能
} else if (GCDHelper.CanUseOffGcd()) {
    // 使用能力技
}
```

### 高级用法
```csharp
// 创建技能对象
var spell = new Spell(123, SpellTargetType.Target);
if (spell.IsReady()) {
    AI.Instance.BattleData.HighPrioritySlots_GCD.Enqueue(spell);
}

// 地面技能
var groundSpell = new Spell(456, target.Position);

// 检查身位
if (Core.Me.IsBehindTarget(target) && target.HasPositional()) {
    // 在背后且目标有身位
}

// 综合判断
if (SpellHelper.CanUseAction() && 
    target.ValidAttackUnit() && 
    target.InRange(Core.Me, 25) &&
    !DotBlacklistHelper.IsBlackList(target)) {
    // 可以对目标使用技能
}
```

## 最佳实践

### 1. 服务缓存
```csharp
public class MyRotation
{
    private readonly MemApiSpell _spellApi;
    private readonly MemApiBuff _buffApi;
    
    public MyRotation()
    {
        _spellApi = Core.Resolve<MemApiSpell>();
        _buffApi = Core.Resolve<MemApiBuff>();
    }
}
```

### 2. 空值检查
```csharp
var target = Core.Me.GetCurrTarget();
if (target?.CanAttack() == true)
{
    // 安全的目标操作
}
```

### 3. 距离模式
```csharp
// 忽略高度和碰撞箱的精确距离
var distance = source.Distance(target, DistanceMode.IgnoreAll);

// 考虑碰撞箱的战斗距离
var combatDistance = source.Distance(target, DistanceMode.IgnoreHeight);
```

### 4. 异常处理
```csharp
try
{
    var spell = Core.Resolve<MemApiSpell>();
    spell.Cast(123, target);
}
catch (Exception ex)
{
    LogHelper.Error($"技能释放失败: {ex.Message}");
}
```

## 注意事项

1. **性能优化**：避免在每帧都调用耗时的API，合理使用缓存
2. **空值检查**：始终检查对象是否为null，特别是目标和队友
3. **状态验证**：在执行操作前检查相关状态（如是否在战斗、是否可移动等）
4. **ID准确性**：确保使用正确的技能ID和Buff ID
5. **线程安全**：大部分API都应在主线程调用
6. **依赖注入**：使用 `Core.Resolve<T>()` 获取服务实例，避免直接new

## 调试技巧

1. 使用 `LogHelper` 输出调试信息
2. 使用 `ToLogString()` 方法获取对象的详细信息
3. 使用 `SpellHelper.PrintSpell()` 打印所有技能信息
4. 检查 `SpellHelper.CanUseAction()` 了解为什么不能使用技能

## 版本说明

- 本文档基于 AEAssist 本体 API，不包含 ACR 目录下的内容
- API 可能会随版本更新而变化，请关注官方更新
- 建议定期检查新增的 API 和功能