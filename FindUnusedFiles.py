#!/usr/bin/env python3

import os, subprocess

rootdir = 'C:\\Users\\ajkno\\Documents\\Game Dev\\Unity\\GMTK-Game-Jam-2020\\Assets'
index = 'C:\\Users\\ajkno\\AppData\\Local\\Entrian Source Search\\Indexes\\OOC'
exceptedFolders = [
	'C:\\Users\\ajkno\\Documents\\Game Dev\\Unity\\GMTK-Game-Jam-2020\\Assets\\NaughtyAttributes-master',
	'C:\\Users\\ajkno\\Documents\\Game Dev\\Unity\\GMTK-Game-Jam-2020\\Assets\\Plugins',
	]
exceptedFiles = [
	'InkLibrary.asset.meta',
	]

for root, dirs, files in os.walk(rootdir):
	for name in files:
		inExceptedFolder = False
		for exceptedFolder in exceptedFolders:
			if (exceptedFolder in root):
				inExceptedFolder = True
				break
		if (name.endswith('.meta') and not inExceptedFolder and not name in exceptedFiles):
			path = os.path.join(root, name)
			if (os.path.isfile(path.removesuffix('.meta'))):
				with open(path) as f:
					for line in f.readlines():
						if (line.startswith('guid')):
							guid = line.split(': ')[1].strip()
							result = subprocess.getoutput(f'ess search -index="{index}" {guid}')
							count = len(result.splitlines())
							if (count <= 1):
								print(f'{path}, {guid}, {count}')