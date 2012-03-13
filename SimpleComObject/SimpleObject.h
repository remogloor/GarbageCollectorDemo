// SimpleObject.h : Declaration of the CSimpleObject

#pragma once
#include "resource.h"       // main symbols



#include "SimpleComObject_i.h"



#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;


// CSimpleObject

class ATL_NO_VTABLE CSimpleObject :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CSimpleObject, &CLSID_SimpleObject>,
	public IDispatchImpl<ISimpleObject, &IID_ISimpleObject, &LIBID_SimpleComObjectLib, /*wMajor =*/ 1, /*wMinor =*/ 0>
{
public:
	CSimpleObject()
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_SIMPLEOBJECT)


BEGIN_COM_MAP(CSimpleObject)
	COM_INTERFACE_ENTRY(ISimpleObject)
	COM_INTERFACE_ENTRY(IDispatch)
END_COM_MAP()



	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}

	void FinalRelease()
	{
	}

public:



};

OBJECT_ENTRY_AUTO(__uuidof(SimpleObject), CSimpleObject)
