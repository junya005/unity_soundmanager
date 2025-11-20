# unity_soundmanager

Unityの音源再生機能のサンプルです。

## バージョン履歴

|バージョン|更新内容|
|----|----|
|var.0.1.0|初期リリース|

## 使い方

パッケージをインストールし、`Assets/Resources`にある`AudioData.asset`に使いたいBGMとSEを設定します。<br>

その後、コード内で以下の記述をすることで音源の再生をすることができます。

```csharp
// BGMの再生
SoundManager.Instance.PlayBGM("オーディオファイル名(拡張子を除く)");
// SEの再生
SoundManager.Instance.PlaySE("オーディオファイル名(拡張子を除く)");
```

## リファレンス

前述の機能のほかに、以下のような機能をそろえております。

### StopBGM

BGMを停止できます。

```csharp
SoundManager.Instance.StopBGM(0.5f);
```

### SetBgmVolume

BGMの音量を設定できます。
この設定はすべてのBGMに影響します。

```csharp
SoundManager.Instance.SetBgmVolume(0.5f);
```

## テスト用音源の著作権について

こちらのリポジトリで公開されている音源は自由に使用して頂いて構いませんが、<br>
著作権は当方に帰属いたします。
