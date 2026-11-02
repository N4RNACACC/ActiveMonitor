# Active Monitor C#
使用.NET C#编写的可配置简易进程监听器   
原为[CXRunfree/Autovisor](https://github.com/CXRunfree/Autovisor)防意外退出设计   
现泛用化修改后公开   
支持nativeAOT   
> 此项目默认启用nativeAOT
   
## 部署与使用
### 使用   
修改`ActiveMonitor/config`内的`config.yml`    
在`monitor`下的`process`和`path`后添加需要监听的进程名和完整路径
> 仅支持监听单个进程

### 本地构建
先决条件：需要安装[.NET SDK](https://dotnet.microsoft.com/download)(.NET 10或以上版本)  
> 请使用.NET SDK而不是Runtime，Runtime无法用于构建
   
1.下载仓库或使用`git clone`克隆本仓库到本地并进入到项目目录
~~~
git clone https://github.com/N4RNACACC/ActiveMonitor
cd ActiveMonitor
~~~   
2.还原Nuget依赖
~~~
dotnet restore
~~~
3.构建项目
~~~
dotnet build -c Release
~~~

## 引用
详见[THIRD_PARTY_LICENSES.txt](THIRD_PARTY_LICENSES.txt)   
## LICENSE
此项目使用MIT License开源，详见[LICENSE.txt](LICENSE.txt)