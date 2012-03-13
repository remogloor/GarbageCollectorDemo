// dllmain.h : Declaration of module class.

class CSimpleComObjectModule : public ATL::CAtlDllModuleT< CSimpleComObjectModule >
{
public :
	DECLARE_LIBID(LIBID_SimpleComObjectLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_SIMPLECOMOBJECT, "{DFBC92C2-94F6-4A49-AF58-8323CEDA67A7}")
};

extern class CSimpleComObjectModule _AtlModule;
