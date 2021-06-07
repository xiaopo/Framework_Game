-- 模块其它部分动态 require
local function gen(name)
	local t = {}
	local values = {} 
	setmetatable( t, 
	{
		__index = function(t,k)
			if k == '__values' then
				return values
			end

			if values[k] and type(values[k]) == 'string' then
				xpcall( function()
					values[k] = require( values[k] )
				end, function(err)
					print_err(string.format( 'MDefine: name=%s k=%s err=%s %s' ,name, values[k],err,debug.traceback( ) ))
					print_err(string.format('lua memory: %s K',collectgarbage("count")))
				end )
				
			end

			return values[k]
		end,

		__newindex = function(t,k,v)
			assert( type(v) == 'string',  string.format( 'MDefine: 值类型必须为 string. name=%s v=%s  type=%s', name,v,type(v) ))
			assert( not values[k],  string.format('MDefine: 已经注册过此路径 name=%s v=%s',name,v) )
			values[k] = v
		end,
	} )

	return t,values
end

local m_cache 	= gen('cache')  -- 缓存数据
local m_proxy 	= gen('proxy')  -- 网络代理

local m_cfg 	= gen('cfg')	-- 配置处理文件
local m_db 		= gen('db')		-- 配置数据表 	

local MDefine = 
{
	cache 	= m_cache,
	cfg  	= m_cfg,
	proxy  	= m_proxy,
	db 		= m_db,
}

_G.MDefine = MDefine
return MDefine
