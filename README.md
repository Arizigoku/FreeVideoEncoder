# フォルダ
## win-x64 windowsパソコン用
## osx-x64 macパソコン用（M1も含む）
## linux-x64 Intel用linux
## linux-arm 主にラズパイ用linux

# コードの書式 
	define 使いたい名前1 使いたい動画ファイルの場所 
	define 使いたい名前2 使いたいオーディオファイルの場所 
	define 使いたい名前3 使いたいフォントの名前 
	video 使いたい名前1 1倍速 始める時間 終わる時間 
	{ 
		text 使いたい名前3 フォントのサイズ 配置する座標 文字の色 
		{ 
			書きたい文字 
		} 
		audio 使いたい名前2 音量 1倍速 始める時間 終わる時間 
		filter 色の強さ #RRGGBB 表示開始時間　表示終了時間 
	} 

# セットアップ
	# osx-x64や、linuxの場合
	export PATH=$PATH:/path/to/FreeVideoEditor/osx-64
	# win-x64の場合
	スタート→コントロールパネルを検索→システムとセキュリティ→システム→システムの詳細設定→システムの環境変数→pathを選択→編集→新規
# コマンドの形式
	FreeVideoEditor コード エンコード場所
	FreeVideoEditor コード エンコード場所 ビデオビットレート
	FreeVideoEditor コード エンコード場所 ビデオビットレート オーディオビットレート
# バグについて
	Issuesで報告してください
