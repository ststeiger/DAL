SELECT 
	oid
	, lowrite(
	lo_open(oid, 131072)
	, (SELECT byteafield FROM tbl WHERE x) 
FROM lo_create(NULL) o(oid); 


-- http://postgresql.1045698.n5.nabble.com/How-to-convert-ByteA-to-Large-Objects-td1914180.html
-- You could turn this around to maybe do: 
-- Question - the number of bytes is not specified in the write - could this be an issue?  
--  lowrite appears undocumented (as opposed to lo_write).
-- We used this to convert the data type to OID. Thank you.

UPDATE tbl 
	SET newoid = 
	( 
		SELECT oid 
		FROM 
		( 
			SELECT oid, lowrite(lo_open(oid, 131072), tbl.byteafield) 
			FROM lo_create(0) o(oid)
		) 
		AS x
	); 
	